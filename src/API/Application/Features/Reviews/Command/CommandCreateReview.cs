using GameService.Domain.Entity.GameReviews;
using MediatR;

namespace GameService.Application.Features.Reviews.Command;

public record CommandCreateReview(
    GameReview GameReview
) : IRequest<GameReview>;