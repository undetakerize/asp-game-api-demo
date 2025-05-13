using System.Reflection;
using GameService.Application.Common;
using GameService.Application.Features.Games.DTO;
using GameService.Domain.Entity.Games;
using GameService.Infrastructure.Data;
using GameService.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Features.Games.Query;

public class GetGameQueryHandler(AppDbContext context) : IRequestHandler<GetGameQuery, Result<PagedResult<GameDto>>>
{
    public async Task<Result<PagedResult<GameDto>>> Handle(GetGameQuery request, CancellationToken cancellationToken)
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
        
        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)search.PageSize);
        var data = await query
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(g => g.ToGameDto())
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<GameDto>
        {
            TotalCount = totalCount,
            TotalPages = totalPages,
            CurrentPage = search.Page,
            PageSize = search.PageSize,
            Data = data
        };
        
        return Result<PagedResult<GameDto>>.Success(pagedResult);
    }
}
