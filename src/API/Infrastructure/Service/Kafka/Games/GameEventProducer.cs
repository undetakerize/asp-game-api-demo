using System.Text.Json;
using Confluent.Kafka;
using GameService.Application.Interfaces;
using GameService.Application.Interfaces.Games;
using GameService.Domain.Entity.Games;
using GameService.Infrastructure.Kafka;
using Microsoft.Extensions.Options;

namespace GameService.Infrastructure.Service.Kafka.Games;

public class GameEventProducer: IGameEventProducer, IDisposable
{
        private readonly IKafkaClientFactory _clientFactory;
        private readonly KafkaSettings _settings;
        private readonly ILogger<GameEventProducer> _logger;
        private readonly IProducer<string, string> _producer;
        private bool _disposed;

        public GameEventProducer(
            IKafkaClientFactory clientFactory,
            IOptions<KafkaSettings> settings,
            ILogger<GameEventProducer> logger)
        {
            _clientFactory = clientFactory;
            _settings = settings.Value;
            _logger = logger;
            _producer = _clientFactory.CreateProducer();
        }

        /// <summary>
        /// Publishes a game created event
        /// </summary>
        public async Task PublishGameCreatedAsync(Game game)
        {
            await PublishMessageAsync(_settings.Topics.GameCreated, game.Id.ToString(), game);
            _logger.LogInformation("Published game created event for game ID: {GameId}", game.Id);
        }

        /// <summary>
        /// Publishes a game updated event
        /// </summary>
        public async Task PublishGameUpdatedAsync(Game game)
        {
            await PublishMessageAsync(_settings.Topics.GameUpdated, game.Id.ToString(), game);
            _logger.LogInformation("Published game updated event for game ID: {GameId}", game.Id);
        }

        /// <summary>
        /// Publishes a game deleted event
        /// </summary>
        public async Task PublishGameDeletedAsync(int gameId)
        {
            await PublishMessageAsync(_settings.Topics.GameDeleted, gameId.ToString(), new { Id = gameId });
            _logger.LogInformation("Published game deleted event for game ID: {GameId}", gameId);
        }

        /// <summary>
        /// Generic method to publish a message to a topic
        /// </summary>
        private async Task PublishMessageAsync<T>(string topic, string key, T message)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(GameEventProducer));

            try
            {
                string serializedMessage = JsonSerializer.Serialize(message);
                
                var deliveryResult = await _producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = key,
                    Value = serializedMessage
                });

                _logger.LogDebug("Delivered message to {Topic} [partition: {Partition}, offset: {Offset}]", 
                    deliveryResult.Topic, deliveryResult.Partition, deliveryResult.Offset);
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError(ex, "Failed to publish message to topic {Topic}", topic);
                throw;
            }
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                _producer?.Flush(TimeSpan.FromSeconds(5));
                _producer?.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
}
