using GameService.Domain.Entity.Reviews;
using GameService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Features.Reviews.Query;

public class GetReviewQueryHandler(AppDbContext context) : IRequestHandler<GetReviewQuery , List<Review>>
{
    public async Task<List<Review>> Handle(GetReviewQuery request, CancellationToken cancellationToken)
    {
        return await context.Review.ToListAsync(cancellationToken);
    }
}
