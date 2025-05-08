using GameService.Domain.Entity.Reviews;

namespace GameService.Application.Features.Reviews.DTO;

public class SyncConsumerReviewDto
{
    public required string Action { get; set; }
    public required int GameId { get; set; }
    public required Review Review { get; set; }
}