using GameService.Domain.Entity.Reviews;
using GameService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Features.Reviews.Command;

public class CommandDeleteReviewHandler(AppDbContext context) : IRequestHandler<CommandDeleteReview , Review?>
{
    public async Task<Review?> Handle(CommandDeleteReview request, CancellationToken cancellationToken)
    {
        var review = await context.Review.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        if (review == null) return review;
        context.Review.Remove(review);
        await context.SaveChangesAsync(cancellationToken);
        return review;
    }
}