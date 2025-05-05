using GameService.Domain.Entity.Reviews;
using GameService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Features.Reviews.Query;

public class GetReviewByIdQueryHandler(AppDbContext context) : IRequestHandler<GetReviewByIdQuery, Review?>
{
    public async Task<Review?> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Review.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
    }
}