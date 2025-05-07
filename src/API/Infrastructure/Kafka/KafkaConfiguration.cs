using GameService.Application.Interfaces;
using GameService.Application.Interfaces.Games;
using GameService.Application.Interfaces.Reviews;
using GameService.Infrastructure.Service;
using GameService.Infrastructure.Service.Kafka.Games;
using GameService.Infrastructure.Service.Kafka.Reviews;

namespace GameService.Infrastructure.Kafka;

public static class KafkaConfiguration
{
    public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configuration
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
            
        // Register core Kafka services
        services.AddSingleton<IKafkaClientFactory, KafkaClientFactoryService>();
            
        // Register producer services
        services.AddSingleton<IGameEventProducer, GameEventProducer>();
            
        //Register consumer services
        services.AddSingleton<IReviewEventConsumer, ReviewEventConsumer>();
        services.AddHostedService<ReviewEventConsumerService>();
        return services;
    }
}