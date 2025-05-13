using GameService.Application.Common;
using GameService.Application.Features.Games.DTO;
using GameService.Helpers.Games;
using MediatR;

namespace GameService.Application.Features.Games.Query;

public record GetGameQuery : IRequest<Result<PagedResult<GameDto>>>
{
    public readonly SearchQueryGame SearchQueryGame;

    public GetGameQuery(SearchQueryGame searchQueryGame)
    {
        SearchQueryGame = searchQueryGame;
    }
}