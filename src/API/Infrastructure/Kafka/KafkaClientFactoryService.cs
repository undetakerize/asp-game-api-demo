using Confluent.Kafka;
using GameService.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace GameService.Infrastructure.Kafka;

public class KafkaClientFactoryService : IKafkaClientFactory
{
        private readonly KafkaSettings _settings;
        private readonly ILogger<KafkaClientFactoryService> _logger;

        public KafkaClientFactoryService(IOptions<KafkaSettings> settings, ILogger<KafkaClientFactoryService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public IProducer<string, string> CreateProducer()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                Acks = Acks.All,
                EnableIdempotence = true,
                MessageSendMaxRetries = 3,
                RetryBackoffMs = 1000,
                MessageTimeoutMs = 5000,  // 5 seconds timeout for message delivery
                SocketTimeoutMs = 3000    // 3 seconds timeout for socket operations
            };

            _logger.LogInformation("Creating Kafka producer with bootstrap servers: {Servers}", _settings.BootstrapServers);
            return new ProducerBuilder<string, string>(config)
                .SetErrorHandler((_, e) => _logger.LogError("Kafka producer error: {Error}", e.Reason))
                .Build();
        }

        public IConsumer<string, string> CreateConsumer(string? groupId = null)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = _settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = _settings.EnableAutoCommit,
                SessionTimeoutMs = _settings.SessionTimeoutMs,
                MaxPollIntervalMs = 300000, // 5 minutes
                EnablePartitionEof = true
            };

            _logger.LogInformation("Creating Kafka consumer with bootstrap servers: {Servers} and group ID: {GroupId}", 
                _settings.BootstrapServers, config.GroupId);
                
            return new ConsumerBuilder<string, string>(config)
                .SetErrorHandler((_, e) => _logger.LogError("Kafka consumer error: {Error}", e.Reason))
                .Build();
        }
    
}
