using GameService.Domain.Entity.GameReviews;
using GameService.Domain.Entity.Reviews;
using GameService.DTO.Review;

namespace GameService.Application.Interfaces.Reviews;

public interface IReviewRepository
{
    Task<List<Review>> GetAllAsync();
    Task<Review?> GetByIdAsync(int id);
    Task<GameReview> CreateReviewAsync(GameReview review);
    Task<Review?> UpdateReviewAsync(int id, UpdateReviewDto dto);
    Task<Review?> DeleteReviewAsync(int id);
    Task<bool> ReviewExistsAsync(int id);
}