namespace GameService.Infrastructure.Kafka;
/// <summary>
/// using GameCreated default topic if not set.
/// </summary>
public class Topics
{
    public string GameCreated { get; set; } = "game-created-topic";
    public string GameUpdated { get; set; } = "game-updated-topic";
    public string GameDeleted { get; set; } = "game-deleted-topic";
    public string ReviewCreatedEvent { get; set; } = "review-consumer-create-topic";
}