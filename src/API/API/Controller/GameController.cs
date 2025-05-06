
using GameService.Application.Features.Games.Command;
using GameService.Application.Features.Games.DTO;
using GameService.Application.Features.Games.Query;
using GameService.Application.Interfaces.Games;
using GameService.Helpers.Games;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameService.Mappers;
using UpdateGameDto = GameService.Application.Features.Games.DTO.UpdateGameDto;

namespace GameService.API.Controller;

[Route("api/game")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IGameEventProducer _gameEventProducer;

    public GameController(IMediator mediator, IGameEventProducer gameEventProducer)
    {
        _mediator = mediator;
        _gameEventProducer = gameEventProducer;
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
        var game = await _mediator.Send(new GetGameByIdQuery(id));
        if (game == null) { 
                return NotFound();
        }
        return Ok(game.ToGameDto());
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameDto dto)
    {
        if(!ModelState.IsValid) return BadRequest();
        try
        {
            var game = await _mediator.Send(dto.ToCommand());
            if (game == null) return BadRequest("Failed to create game");
            await _gameEventProducer.PublishGameCreatedAsync(game);
            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game.ToGameDto());
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{id:int}/update")]
    [Authorize]
    public async Task<IActionResult> UpdateGame([FromRoute] int id, [FromBody] UpdateGameDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        try
        {
            var game = await _mediator.Send(dto.ToUpdateCommand(id));
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game.ToGameDto());
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("{id:int}/delete")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest();
        var game = await _mediator.Send(new CommandDeleteGame(id));
        if (game == null) return NotFound("Game Doesnt exist");
        return Ok(game);
    }
}

