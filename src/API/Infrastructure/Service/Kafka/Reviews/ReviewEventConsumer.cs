using Confluent.Kafka;
using GameService.Application.Features.Reviews.Command;
using GameService.Application.Features.Reviews.DTO;
using GameService.Application.Interfaces;
using GameService.Application.Interfaces.Reviews;
using GameService.Infrastructure.Common;
using GameService.Infrastructure.Kafka;
using GameService.Mappers;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;

namespace GameService.Infrastructure.Service.Kafka.Reviews;

public sealed class ReviewEventConsumer : IReviewEventConsumer, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly KafkaSettings _settings;
    private readonly ILogger<ReviewEventConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;
    private bool _disposed;
    
    // Configuration constants
    private const int ConsumeTimeoutMs = 1000;
    private const int ShortDelayMs = 100;
    private const int ErrorRetryDelayMs = 1000;

    public ReviewEventConsumer(
        IServiceScopeFactory scopeFactory,
        IKafkaClientFactory clientFactory,
        IOptions<KafkaSettings> settings,
        ILogger<ReviewEventConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _settings = settings.Value;
        _logger = logger;
        _consumer = clientFactory.CreateConsumer();
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Kafka consumer for topic: {Topic}", _settings.Topics.ReviewCreatedEvent);
        _consumer.Subscribe(_settings.Topics.ReviewCreatedEvent);

        try
        {
            await RunConsumerLoop(cancellationToken);
        }
        catch (OperationCanceledException e)
        {
            _logger.LogInformation(e, "Kafka consumer stopped due to cancellation");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in Kafka consumer");
            throw;
        }
        finally
        {
            SafeCloseConsumer();
        }
    }

    private async Task RunConsumerLoop(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && !_disposed)
        {
            try
            {
                var consumeResult = TryConsumeMessage(cancellationToken);
                if (consumeResult == null) continue;
                
                if (consumeResult.IsPartitionEOF)
                {
                    LogPartitionEnd(consumeResult);
                    continue;
                }

                await ProcessMessage(consumeResult);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                HandleConsumerException(ex);
                await Task.Delay(ErrorRetryDelayMs, cancellationToken);
            }
        }
    }

    private ConsumeResult<string, string>? TryConsumeMessage(CancellationToken cancellationToken)
    {
        try
        {
            var result = _consumer.Consume(ConsumeTimeoutMs);
            
            if (result == null)
            {
                Task.Delay(ShortDelayMs, cancellationToken).Wait(cancellationToken);
                return null;
            }
            
            return result;
        }
        catch (ConsumeException ex) when (ex.Error.Code == ErrorCode.Local_TimedOut)
        {
            // Expected timeout when no messages available
            Task.Delay(ShortDelayMs, cancellationToken).Wait(cancellationToken);
            return null;
        }
    }

    private async Task ProcessMessage(ConsumeResult<string, string> consumeResult)
    {
        _logger.LogDebug("Processing message: [Partition: {Partition}, Offset: {Offset}]",
            consumeResult.Partition, consumeResult.Offset);

        var reviewEvent = JsonConvert.DeserializeObject<SyncConsumerReviewDto>(
            consumeResult.Message.Value,
            NewtonsoftJsonSettings.CaseInsensitive);
        
        if (reviewEvent != null)
        {
            await ConsumeReviewAsync(reviewEvent);
        }
        else
        {
            _logger.LogWarning("Received invalid review event message");
        }
        
        CommitOffset(consumeResult);
    }

    private void CommitOffset(ConsumeResult<string, string> consumeResult)
    {
        try
        {
            _consumer.Commit(consumeResult);
            _logger.LogDebug("Committed offset: [Partition: {Partition}, Offset: {Offset}]",
                consumeResult.Partition, consumeResult.Offset);
        }
        catch (KafkaException ex)
        {
            _logger.LogError(ex, "Error committing offset");
        }
    }

    private void LogPartitionEnd(ConsumeResult<string, string> consumeResult)
    {
        _logger.LogInformation("Reached end of partition: [Partition: {Partition}, Offset: {Offset}]",
            consumeResult.Partition, consumeResult.Offset);
    }

    private void HandleConsumerException(Exception ex)
    {
        if (ex is ConsumeException kafkaEx)
        {
            _logger.LogError(ex, "Kafka consume error: {Reason}", kafkaEx.Error.Reason);
        }
        else if (ex is JsonException)
        {
            _logger.LogError(ex, "Error deserializing review event");
        }
        else
        {
            _logger.LogError(ex, "Unexpected error during message processing");
        }
    }

    public async Task ConsumeReviewAsync(SyncConsumerReviewDto? dto)
    {
        if (dto == null)
        {
            _logger.LogWarning("Review event was null");
            return;
        }
        
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        try
        {
            if (dto.Action.Equals("create", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Processing review creation for : {@Dto}", JsonConvert.SerializeObject(dto));
                var gameReviewDto = dto.ToCreateReviewDto().ToReviewFromCreateDto(dto.GameId);
                await mediator.Send(new CommandCreateReview(gameReviewDto.gameReview));
                _logger.LogInformation("Successfully processed review: {ReviewId}", gameReviewDto.review.Id);
            }
            else
            {
                _logger.LogWarning("Unknown action type: {Action} for review: {@Dto}", 
                    dto.Action, dto);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing review");
            throw;
        }
    }

    private void SafeCloseConsumer()
    {
        try
        {
            _logger.LogInformation("Closing Kafka consumer");
            _consumer.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error closing Kafka consumer");
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            try
            {
                _consumer.Close();
                _consumer.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing Kafka consumer");
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}
