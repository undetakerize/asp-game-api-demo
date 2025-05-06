using Confluent.Kafka;
using GameService.Domain.Entity.Games;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GameService.Infrastructure.Service;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConfiguration? _configuration;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly string? _topic;
    private readonly string? _bootstrapServers;
    private readonly string _groupId;
    private IConsumer<string, string>? _consumer;
    private bool _isInitialized = false;

    public KafkaConsumerService(IConfiguration configuration, ILogger<KafkaConsumerService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        _bootstrapServers = _configuration["Kafka:BootstrapServers"];
        _topic = _configuration["Kafka:Topics:GameCreated"];
        _groupId = _configuration["Kafka:GroupId"] ?? "game-consumer-group";
        
        // Validate configuration
        if (string.IsNullOrEmpty(_bootstrapServers))
        {
            _logger.LogWarning("Kafka BootstrapServers configuration is missing or empty.");
        }
        
        if (string.IsNullOrEmpty(_topic))
        {
            _logger.LogWarning("Kafka Topics:GameCreated configuration is missing or empty.");
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("KafkaConsumerService is starting");
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Delay startup slightly to avoid interfering with application startup
        await Task.Delay(2000, stoppingToken);
        
        // Check if configuration is valid
        if (string.IsNullOrEmpty(_bootstrapServers) || string.IsNullOrEmpty(_topic))
        {
            _logger.LogError("Kafka configuration is invalid. Service will not start.");
            return; // Exit the service
        }

        _logger.LogInformation("Initializing Kafka consumer with bootstrap servers: {Servers}", _bootstrapServers);

        try
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = $"{_groupId}-{Guid.NewGuid()}", // Generate a unique consumer group ID
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                // Set timeouts to help with faster shutdown
                SessionTimeoutMs = 6000,
                ConnectionsMaxIdleMs = 10000,
                // Add error handling
                //ErrorOnNoTopicCreation = false,
                AllowAutoCreateTopics = true
            };

            // Create and configure the consumer
            try
            {
                _consumer = new ConsumerBuilder<string, string>(config)
                    .SetErrorHandler((_, e) => _logger.LogError("Kafka error: {Error}", e.Reason))
                    .SetLogHandler((_, e) => _logger.LogDebug("Kafka log: {Message}", e.Message))
                    .Build();
                
                _consumer.Subscribe(_topic);
                _isInitialized = true;
                _logger.LogInformation("Kafka consumer started. Listening to topic: {Topic}", _topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Kafka consumer. Service will not start.");
                return; // Exit the service
            }

            // Main processing loop
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Use a short timeout to ensure we check for cancellation frequently
                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));
                    
                    if (consumeResult == null)
                        continue;

                    _logger.LogInformation("Received message: {Message}", consumeResult.Message.Value);

                    try
                    {
                        var game = JsonSerializer.Deserialize<Game>(consumeResult.Message.Value);
                        if (game != null)
                        {
                            _logger.LogInformation("Game ID: {Id}, Title: {Title}, Price: {Price}, Desc: {Desc}",
                                game.Id, game.Title, game.Price, game.Description);
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "Deserialization error for message: {Message}", consumeResult.Message.Value);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming message from Kafka");
                }
                catch (OperationCanceledException)
                {
                    // This is expected during shutdown
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in Kafka consumer");
                    // Add a short delay to prevent tight error loop
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // This is expected during shutdown
            _logger.LogInformation("Kafka consumer stopping due to cancellation");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fatal error in Kafka consumer service");
        }
        finally
        {
            CleanupConsumer();
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Kafka consumer service");
        
        CleanupConsumer();
        
        await base.StopAsync(cancellationToken);
        _logger.LogInformation("Kafka consumer service stopped");
    }
    
    private void CleanupConsumer()
    {
        if (_isInitialized && _consumer != null)
        {
            try
            {
                _logger.LogInformation("Closing Kafka consumer");
                _consumer.Close();
                _consumer.Dispose();
                _consumer = null;
                _isInitialized = false;
                _logger.LogInformation("Kafka consumer closed and disposed");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error during Kafka consumer cleanup");
            }
        }
    }
}
