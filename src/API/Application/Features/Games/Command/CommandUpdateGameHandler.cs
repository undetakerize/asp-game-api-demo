using GameService.Domain.Entity.Games;
using GameService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Features.Games.Command;

public class CommandUpdateGameHandler(AppDbContext context) : IRequestHandler<CommandUpdateGame, Game?>
{
    public async Task<Game?> Handle(CommandUpdateGame request, CancellationToken cancellationToken)
    {
        var game = await context.Game.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
        if (game == null) return game;
        
        game.Title = request.Title;
        game.Description = request.Description;
        game.ReleaseDate = request.ReleaseDate;
        game.Price = request.Price;
        await context.SaveChangesAsync(cancellationToken);
        return game;
    }
}