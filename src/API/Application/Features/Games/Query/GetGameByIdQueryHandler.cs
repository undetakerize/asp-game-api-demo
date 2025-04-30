using GameService.Domain.Entity.Games;
using GameService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Features.Games.Query;

public class GetGameByIdQueryHandler(AppDbContext context) : IRequestHandler<GetGameByIdQuery, Game?>
{
    public async Task<Game?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Game.Include(r => r.GameReviews)
            .ThenInclude(r => r.Review)
            .FirstOrDefaultAsync(i => request.Id == i.Id, cancellationToken);
    }
}
