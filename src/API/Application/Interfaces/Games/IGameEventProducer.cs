using GameService.Domain.Entity.Games;

namespace GameService.Application.Interfaces.Games;

public interface IGameEventProducer
{
    Task PublishGameCreatedAsync(Game game);
    // Task PublishGameUpdatedAsync(Game game);
    // Task PublishGameDeletedAsync(int gameId);
}