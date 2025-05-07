using GameService.Application.Features.Reviews.DTO;
using GameService.Domain.Entity.Games;
using GameService.Domain.Entity.Reviews;

namespace GameService.Application.Interfaces.Reviews;

public interface IReviewEventConsumer
{
    Task ConsumeReviewAsync(SyncConsumerReviewDto dto);
}