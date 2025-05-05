using GameService.Domain.Entity.Reviews;
using GameService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Features.Reviews.Command;

public class CommandUpdateReviewHandler(AppDbContext context) : IRequestHandler<CommandUpdateReview, Review?>
{
    public async Task<Review?> Handle(CommandUpdateReview request, CancellationToken cancellationToken)
    {
        var review = await context.Review.FirstOrDefaultAsync(r => r.Id == request.Id,  cancellationToken);
        if (review == null) return review;
        review.Title = request.Title;
        review.Content = request.Content;
        await context.SaveChangesAsync(cancellationToken);
        return review;
    }
}