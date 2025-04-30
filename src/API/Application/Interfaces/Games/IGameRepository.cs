using GameService.Domain.Entity.Games;
using GameService.Helpers.Games;
using Lombok.NET;
using DTO_UpdateGameDto = GameService.Application.Features.Games.DTO.UpdateGameDto;
using UpdateGameDto = GameService.Application.Features.Games.DTO.UpdateGameDto;

namespace GameService.Application.Interfaces.Games;

[AsyncOverloads]
public partial interface IGameRepository
{
    List<Game> GetAll(SearchQueryGame searchQueryGame); 
    int GetAllCount(SearchQueryGame searchQueryGame);
    Game? GetById(int id);
    Game CreateGame(Game game);
    Game? UpdateGame(int id, DTO_UpdateGameDto dto);
    Game? DeleteGame(int id);
    bool GameExists(int id);
}