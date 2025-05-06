using GameService.Application.Interfaces;
using GameService.Domain.Entity.Games;
using GameService.Infrastructure.Data;
using MediatR;

namespace GameService.Application.Features.Games.Command;

public class CommandCreateGameHandler(AppDbContext context) : IRequestHandler<CommandCreateGame, Game?>
{
    public async Task<Game?> Handle(CommandCreateGame request, CancellationToken cancellationToken)
    {
        var game = new Game
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseDate = request.ReleaseDate,
            Price = request.Price
        };

        await context.Game.AddAsync(game, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return game;
    }
}
