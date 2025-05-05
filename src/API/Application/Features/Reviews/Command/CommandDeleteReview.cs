using GameService.Domain.Entity.Reviews;
using MediatR;

namespace GameService.Application.Features.Reviews.Command;

public record CommandDeleteReview(
    int Id
) : IRequest<Review?>;