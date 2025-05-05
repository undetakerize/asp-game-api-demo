using GameService.Domain.Entity.GameReviews;
using GameService.Infrastructure.Data;
using MediatR;

namespace GameService.Application.Features.Reviews.Command;

public class CommandCreateReviewHandler (AppDbContext context) : IRequestHandler<CommandCreateReview, GameReview>
{
    public async Task<GameReview> Handle(CommandCreateReview request, CancellationToken cancellationToken)
    {
        await context.Review.AddAsync(request.GameReview.Review,cancellationToken);
        await context.GameReview.AddAsync(request.GameReview, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return request.GameReview;
    }
}