using GameService.Domain.Entity.Games;
using GameService.Helpers.Games;
using MediatR;

namespace GameService.Application.Features.Games.Query;

public class GetGameQuery : IRequest<List<Game>>
{
    public SearchQueryGame SearchQueryGame { get; set; }

    public GetGameQuery(SearchQueryGame searchQueryGame)
    {
        SearchQueryGame = searchQueryGame;
    }
    
}