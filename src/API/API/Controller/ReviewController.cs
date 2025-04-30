using GameService.Application.Interfaces.Reviews;
using GameService.DTO.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameService.Mappers;
using Games_IGameRepository = GameService.Application.Interfaces.Games.IGameRepository;
using IGameRepository = GameService.Application.Interfaces.Games.IGameRepository;

namespace GameService.API.Controller;

[Route("api/review")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IGameRepository _gameRepository;

    public ReviewController(IReviewRepository reviewRepository, Games_IGameRepository gameRepository)
    {
        _reviewRepository = reviewRepository;
        _gameRepository = gameRepository;
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
        if (!await _gameRepository.GameExistsAsync(gameId)) return BadRequest();
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