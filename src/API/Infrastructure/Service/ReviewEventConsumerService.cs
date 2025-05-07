using GameService.Application.Interfaces.Reviews;
using GameService.Infrastructure.Service.Kafka.Reviews;

namespace GameService.Infrastructure.Service;

public class ReviewEventConsumerService(IReviewEventConsumer consumer) : BackgroundService
{
    private readonly ReviewEventConsumer _consumer = consumer as ReviewEventConsumer 
                                                     ?? throw new InvalidOperationException("Consumer must be ReviewEventConsumer.");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.StartConsumingAsync(stoppingToken);
    }
}
