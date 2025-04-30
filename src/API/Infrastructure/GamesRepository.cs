using System.Reflection;
using GameService.Domain.Entity.Games;
using GameService.Helpers.Games;
using GameService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DTO_UpdateGameDto = GameService.Application.Features.Games.DTO.UpdateGameDto;
using IGameRepository = GameService.Application.Interfaces.Games.IGameRepository;
using UpdateGameDto = GameService.Application.Features.Games.DTO.UpdateGameDto;

namespace GameService.Infrastructure;

public class GameRepository(AppDbContext context): IGameRepository
{
    public Task<List<Game>> GetAllAsync(SearchQueryGame searchQueryGame, CancellationToken cancellationToken = default)
    {
        var query = context.Game
            .Include(g => g.GameReviews)
            .ThenInclude(r => r.Review)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQueryGame.Keywords))
        {
            var keyword = $"%{searchQueryGame.Keywords}%";

            query = query.Where(g =>
                EF.Functions.Like(g.Title, keyword) ||
                EF.Functions.Like(g.Description, keyword));
            
            if (decimal.TryParse(searchQueryGame.Keywords, out var price))
            {
                query = query.Where(g => g.Price == price);
            }
        }

        if (!string.IsNullOrWhiteSpace(searchQueryGame.Title))
        {
            query = query.Where(g => g.Title.ToLower().Contains(searchQueryGame.Title.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(searchQueryGame.Sort))
        {
            var sortParts = searchQueryGame.Sort.Split(',');
            if (sortParts.Length == 2)
            {
                var propertyName = sortParts[0];
                var sortDirection = sortParts[1].ToLower();

                var property = typeof(Game).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    query = sortDirection == "asc"
                        ? query.OrderBy(g => EF.Property<object>(g, property.Name))
                        : query.OrderByDescending(g => EF.Property<object>(g, property.Name));
                }
            }
        }

        return query.ToListAsync(cancellationToken);
    }

    public Task<int> GetAllCountAsync(SearchQueryGame searchQueryGame, CancellationToken cancellationToken = default)
    {
        var query = context.Game.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQueryGame.Title))
        {
            query = query.Where(g => g.Title!= String.Empty && g.Title.Contains(searchQueryGame.Title));
        }

        return query.CountAsync(cancellationToken);
    }

    public Task<Game?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return context.Game.Include(r => r.GameReviews)
            .ThenInclude(r => r.Review)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<Game> CreateGameAsync(Game game, CancellationToken cancellationToken = default)
    {
         await context.Game.AddAsync(game, cancellationToken);
         await context.SaveChangesAsync(cancellationToken);
         return game;
    }

    public async Task<Game?> UpdateGameAsync(int id, UpdateGameDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Game?> DeleteGameAsync(int id, CancellationToken cancellationToken = default)
    {
        var game = await context.Game.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        if (game == null) return game;
        context.Game.Remove(game);
        await context.SaveChangesAsync(cancellationToken);
        return game;
    }

    public async Task<bool> GameExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var game = await GetByIdAsync(id, cancellationToken);
        return game != null;
    }

    public List<Game> GetAll(SearchQueryGame searchQueryGame)
    {
        return context.Game.Include(r => r.GameReviews)
            .ThenInclude(gr => gr.Review)
            .ToList();
    }

    public int GetAllCount(SearchQueryGame searchQueryGame)
    {
        throw new NotImplementedException();
    }

    public Game? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Game CreateGame(Game game)
    {
        throw new NotImplementedException();
    }

    public Game? UpdateGame(int id, DTO_UpdateGameDto dto)
    {
        throw new NotImplementedException();
    }

    public Game? DeleteGame(int id)
    {
        throw new NotImplementedException();
    }

    public bool GameExists(int id)
    {
        throw new NotImplementedException();
    }
}