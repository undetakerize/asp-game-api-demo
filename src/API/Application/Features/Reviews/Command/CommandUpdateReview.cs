using GameService.Domain.Entity.Reviews;
using MediatR;

namespace GameService.Application.Features.Reviews.Command;

public record CommandUpdateReview
(
    int Id,
    string Title,
    string Content
) : IRequest<Review?>;