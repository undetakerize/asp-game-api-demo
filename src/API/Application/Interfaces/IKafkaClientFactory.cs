using Confluent.Kafka;

namespace GameService.Application.Interfaces;

public interface IKafkaClientFactory
{
    IProducer<string, string> CreateProducer();
    IConsumer<string, string> CreateConsumer(string? groupId = null);
}