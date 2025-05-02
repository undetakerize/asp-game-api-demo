using GameService.Domain.Entity.Games;
using GameService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Features.Games.Command;

public class CommandDeleteGameHandler(AppDbContext context) : IRequestHandler<CommandDeleteGame, Game?>
{
    public async Task<Game?> Handle(CommandDeleteGame request, CancellationToken cancellationToken)
    {
        var game = await context.Game.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (game is null) return game;
        context.Game.Remove(game);
        await context.SaveChangesAsync(cancellationToken);
        return game;
    }
}
