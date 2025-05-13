
using Confluent.Kafka;
using GameService.Application.Common;
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
    private readonly ILogger<GameController> _logger;
    public GameController(IMediator mediator, IGameEventProducer gameEventProducer, ILogger<GameController> logger)
    {
        _mediator = mediator;
        _gameEventProducer = gameEventProducer;
        _logger = logger;
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
        var result = await _mediator.Send(new GetGameQuery(searchQueryGame), cancellationToken);
        return result.ToHttpResult();
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
        var game = await _mediator.Send(dto.ToCommand());
        if (game == null) return BadRequest("Failed to create game");
        try
        {
            await _gameEventProducer.PublishGameCreatedAsync(game);
        }
        catch (Exception e) when (e is TimeoutException || e is KafkaException)
        {
            // Log the error but don't fail the request
            _logger.LogError(e, "Failed to publish game created event for game {GameId}. Event will need to be recovered.", game.Id);
        }
        return CreatedAtAction(nameof(GetById), new { id = game.Id }, game.ToGameDto());
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

