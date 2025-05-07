using System.Text.Json;
using Confluent.Kafka;
using GameService.Application.Features.Reviews.DTO;
using GameService.Application.Interfaces;
using GameService.Application.Interfaces.Reviews;
using GameService.Infrastructure.Kafka;
using Microsoft.Extensions.Options;

namespace GameService.Infrastructure.Service.Kafka.Reviews;

public sealed class ReviewEventConsumer : IReviewEventConsumer, IDisposable
{
    private readonly KafkaSettings _settings;
    private readonly ILogger<ReviewEventConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;
    private bool _disposed;

    public ReviewEventConsumer(
        IKafkaClientFactory clientFactory,
        IOptions<KafkaSettings> settings,
        ILogger<ReviewEventConsumer> logger)
    {
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

                    var reviewEvent = JsonSerializer.Deserialize<SyncConsumerReviewDto>(consumeResult.Message.Value);
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

        switch (dto.Action.ToLower())
        {
            case "create":
                _logger.LogInformation("Handling 'create' action for Review ID: {ReviewId}", dto.Review?.Id);
                break;
            default:
                _logger.LogWarning("Unknown action type: {Action}", dto.Action);
                return;
        }

        await Task.Delay(500); // Simulate async processing
        _logger.LogInformation("Finished processing review event for Review ID: {ReviewId}", dto.Review?.Id);
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
