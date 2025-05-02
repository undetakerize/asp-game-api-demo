using GameService.Domain.Entity.Games;
using GameService.Helpers.Games;
using MediatR;

namespace GameService.Application.Features.Games.Query;

public record GetGameQuery : IRequest<List<Game>>
{
    public readonly SearchQueryGame SearchQueryGame;

    public GetGameQuery(SearchQueryGame searchQueryGame)
    {
        SearchQueryGame = searchQueryGame;
    }
    
}