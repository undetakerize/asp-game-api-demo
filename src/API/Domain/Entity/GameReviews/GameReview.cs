using GameService.Domain.Entity.Games;
using GameService.Domain.Entity.Reviews;

namespace GameService.Domain.Entity.GameReviews;

public class GameReview
{
    public int GameId { get; set; }
    public Game Game { get; set; }
    public int ReviewId { get; set; }
    public Review Review { get; set; }
}