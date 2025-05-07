using System.Globalization;
using GameService.Domain.Entity.Games;
using GameService.Infrastructure.Data;
using MediatR;

namespace GameService.Application.Features.Games.Command;

public class CommandCreateGameHandler(AppDbContext context) : IRequestHandler<CommandCreateGame, Game?>
{
    public async Task<Game?> Handle(CommandCreateGame request, CancellationToken cancellationToken)
    {
        
        DateTime? releaseDate = null;
        if (!string.IsNullOrEmpty(request.ReleaseDate))
        {
            releaseDate = DateTime.ParseExact(
                request.ReleaseDate,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture
            );
            releaseDate = DateTime.SpecifyKind(releaseDate.Value, DateTimeKind.Utc);
        }
        var game = new Game
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseDate = releaseDate,
            Price = request.Price
        };

        await context.Game.AddAsync(game, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return game;
    }
}
