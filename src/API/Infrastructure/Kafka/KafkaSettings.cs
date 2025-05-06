using Confluent.Kafka;

namespace GameService.Infrastructure.Kafka;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = "localhost:8100";
    public string GroupId { get; set; } = "default-group";
    public Topics Topics { get; set; } = new Topics();
    public int SessionTimeoutMs { get; set; } = 10000;
    public int AutoOffsetResetValue { get; set; } = (int)AutoOffsetReset.Earliest;
    public bool EnableAutoCommit { get; set; } = true;
}