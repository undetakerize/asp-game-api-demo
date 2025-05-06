using GameService.Application.Interfaces;
using GameService.Application.Interfaces.Games;
using GameService.Infrastructure.Service.Kafka.Games;

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
            
        // Register consumer services
        //services.AddHostedService<GameEventConsumerService>();
            
        return services;
    }
}