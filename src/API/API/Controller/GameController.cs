using GameService.Application.Features.Games.DTO;
using GameService.Application.Features.Games.Query;
using GameService.Helpers.Games;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameService.Mappers;
using DTO_UpdateGameDto = GameService.Application.Features.Games.DTO.UpdateGameDto;
using Games_IGameRepository = GameService.Application.Interfaces.Games.IGameRepository;
using IGameRepository = GameService.Application.Interfaces.Games.IGameRepository;
using UpdateGameDto = GameService.Application.Features.Games.DTO.UpdateGameDto;

namespace GameService.API.Controller;

[Route("api/game")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly Games_IGameRepository _gameRepository;
    private readonly IMediator _mediator;

    public GameController(Games_IGameRepository gameRepository, IMediator mediator)
    {
        _gameRepository = gameRepository;
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(
        [FromQuery] SearchQueryGame searchQueryGame,
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return BadRequest();
        var game = await _mediator.Send(new GetGameQuery(searchQueryGame), cancellationToken);
        
        var totalCount = game.Count;
        var totalPages = (int)Math.Ceiling((double)game.Count / pageSize);
        game = game.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var result = new
        {
            TotalCount = totalCount,
            TotalPages = totalPages,
            CurrentPage = page,
            PageSize = pageSize,
            Data = game.Select(g => g.ToGameDto())
        };
        return Ok(result); 
    }
    
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest();
        var game = await _mediator.Send(new GetGameByIdQuery{Id = id});
        if (game == null) { 
                return NotFound();
        }
        return Ok(game.ToGameDto());
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        var game = dto.ToGameFromCreateDto();
        await _gameRepository.CreateGameAsync(game);
        return CreatedAtAction(nameof(GetById), new { id = game.Id }, game.ToGameDto());
    }

    [HttpPut("{id:int}/update")]
    [Authorize]
    public async Task<IActionResult> UpdateGame([FromRoute] int id, [FromBody] DTO_UpdateGameDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        var game = await _gameRepository.UpdateGameAsync(id, dto);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game.ToGameDto());
    }

    [HttpDelete("{id:int}/delete")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest();
        var game = await _gameRepository.DeleteGameAsync(id);
        if (game == null) return NotFound("Game Doesnt exist");
        return Ok(game);
    }
}

