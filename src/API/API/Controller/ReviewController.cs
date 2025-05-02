using GameService.Application.Features.Games.Query;
using GameService.Application.Interfaces.Reviews;
using GameService.DTO.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameService.Mappers;
using MediatR;

namespace GameService.API.Controller;

[Route("api/review")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMediator _mediator;

    public ReviewController(IReviewRepository reviewRepository, IMediator mediator)
    {
        _reviewRepository = reviewRepository;
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllReviews()
    {
        if (!ModelState.IsValid) return BadRequest();
        var reviews = await _reviewRepository.GetAllAsync();
        var response = reviews.Select(r => r.ToReviewDto());
        return Ok(response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdReview([FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest();
        var review = await _reviewRepository.GetByIdAsync(id);
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
        await _reviewRepository.CreateReviewAsync(gameReviewDto.gameReview);
        return CreatedAtAction(nameof(GetByIdReview), new { id = gameReviewDto.review.Id }, gameReviewDto.review.ToReviewDto());
    }

    [HttpPut("{id:int}/update")]
    [Authorize]
    public async Task<IActionResult> UpdateReview(int id, UpdateReviewDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        var review = await _reviewRepository.UpdateReviewAsync(id, dto);
        if(review == null) return NotFound();
        return Ok(review.ToReviewDto());
    }

    [HttpDelete("{id:int}/delete")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(int id)
    {
        if (!ModelState.IsValid) return BadRequest();
        var review = await _reviewRepository.DeleteReviewAsync(id);
        if(review == null) return NotFound("Review not found");
        return Ok(review);
    }
}