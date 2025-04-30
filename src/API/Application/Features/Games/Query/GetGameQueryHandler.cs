using System.Reflection;
using GameService.Domain.Entity.Games;
using GameService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Features.Games.Query;

public class GetGameQueryHandler(AppDbContext context) : IRequestHandler<GetGameQuery, List<Game>>
{
    public async Task<List<Game>> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var query = context.Game
            .Include(g => g.GameReviews)
            .ThenInclude(r => r.Review)
            .AsQueryable();

        var search = request.SearchQueryGame;

        if (!string.IsNullOrWhiteSpace(search.Keywords))
        {
            var keyword = $"%{search.Keywords}%";

            query = query.Where(g =>
                EF.Functions.Like(g.Title, keyword) ||
                EF.Functions.Like(g.Description, keyword));
            
            if (decimal.TryParse(search.Keywords, out var price))
            {
                query = query.Where(g => g.Price == price);
            }
        }

        if (!string.IsNullOrWhiteSpace(search.Title))
        {
            query = query.Where(g => g.Title.ToLower().Contains(search.Title.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(search.Sort))
        {
            var sortParts = search.Sort.Split(',');
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
        return await query.ToListAsync(cancellationToken);
    }
}
