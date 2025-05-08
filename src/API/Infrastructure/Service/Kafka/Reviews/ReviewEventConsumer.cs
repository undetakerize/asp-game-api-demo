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
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace GameService.Infrastructure.Service.Kafka.Reviews;

public sealed class ReviewEventConsumer : IReviewEventConsumer, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly KafkaSettings _settings;
    private readonly ILogger<ReviewEventConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;
    private bool _disposed;

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

    /// <summary>
    /// Starts consuming ReviewEvent messages from Kafka using a cancellation token.
    /// Should be called from a BackgroundService or hosted environment.
    /// </summary>
    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Kafka consumer subscribing to topic: {Topic}", _settings.Topics.ReviewCreatedEvent);
        _consumer.Subscribe(_settings.Topics.ReviewCreatedEvent);

        try
        {
            while (!cancellationToken.IsCancellationRequested && !_disposed)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    if (consumeResult.IsPartitionEOF)
                    {
                        _logger.LogInformation("Reached end of partition: {Topic} [Partition: {Partition}, Offset: {Offset}]",
                            consumeResult.Topic, consumeResult.Partition, consumeResult.Offset);
                        continue;
                    }

                    var reviewEvent = JsonConvert.DeserializeObject<SyncConsumerReviewDto>(
                        consumeResult.Message.Value,
                        NewtonsoftJsonSettings.CaseInsensitive);
                    
                    if (reviewEvent != null)
                    {
                        await ConsumeReviewAsync(reviewEvent);
                    }
                    else
                    {
                        _logger.LogWarning("Received invalid review event message.");
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Kafka consume error: {Reason}", ex.Error.Reason);
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "Error deserializing review event.");
                }
            }
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Kafka consumer stopped due to cancellation.");
        }
        finally
        {
            _consumer.Close();
        }
    }

    public async Task ConsumeReviewAsync(SyncConsumerReviewDto? dto)
    {
        if (dto == null)
        {
            _logger.LogWarning("Review event was null.");
            return;
        }
        
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        switch (dto.Action.ToLower())
        {
            case "create":
                _logger.LogInformation("Handling {Action} action for Review ID: {@Dto}", dto.Action, JsonConvert.SerializeObject(dto));
                var gameReviewDto = dto.ToCreateReviewDto().ToReviewFromCreateDto(dto.GameId);
                await mediator.Send(new CommandCreateReview(gameReviewDto.gameReview));
                break;
            default:
                _logger.LogWarning("Unknown action type: {Action}", dto.Action);
                return;
        }
        await Task.Delay(500); // Simulate async processing
        _logger.LogInformation("Finished processing review event for Review ID: {@Dto}", JsonConvert.SerializeObject(dto));
    }
    public void Dispose()
    {
        if (!_disposed)
        {
            _consumer.Dispose();
            _disposed = true;
        }
    }
}
