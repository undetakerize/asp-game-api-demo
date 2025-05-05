using GameService.Application.Features.Games.Query;
using GameService.Application.Features.Reviews.Command;
using GameService.Application.Features.Reviews.DTO;
using GameService.Application.Features.Reviews.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameService.Mappers;
using MediatR;
using CreateReviewDto = GameService.Application.Features.Reviews.DTO.CreateReviewDto;

namespace GameService.API.Controller;

[Route("api/review")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllReviews()
    {
        if (!ModelState.IsValid) return BadRequest();
        var reviews = await _mediator.Send(new GetReviewQuery());
        var response = reviews.Select(r => r.ToReviewDto());
        return Ok(response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdReview([FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest();
        var review = await _mediator.Send(new GetReviewByIdQuery(id));
        if (review == null)
        {
            return NotFound();
        }
        return Ok(review.ToReviewDto());
    }

    [HttpPost("{gameId:int}")]
    [Authorize]
    public async Task<IActionResult> CreateReview(int gameId, [FromBody] CreateReviewDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (await _mediator.Send(new GetGameByIdQuery(gameId)) == null) return BadRequest();
        var gameReviewDto = dto.ToReviewFromCreateDto(gameId);
        await _mediator.Send(new CommandCreateReview(gameReviewDto.gameReview));
        return CreatedAtAction(nameof(GetByIdReview), new { id = gameReviewDto.review.Id }, gameReviewDto.review.ToReviewDto());
    }

    [HttpPut("{id:int}/update")]
    [Authorize]
    public async Task<IActionResult> UpdateReview(int id, UpdateReviewDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        var review = _mediator.Send(new CommandUpdateReview(id, dto.Title, dto.Content));
        if(await review == null) return NotFound();
        return Ok(review);
    }

    [HttpDelete("{id:int}/delete")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(int id)
    {
        if (!ModelState.IsValid) return BadRequest();
        var review = await _mediator.Send(new CommandDeleteReview(id));
        if(review == null) return NotFound("Review not found");
        return Ok(review);
    }
}