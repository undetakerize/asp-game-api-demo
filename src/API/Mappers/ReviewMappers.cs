using GameService.Domain.Entity.GameReviews;
using GameService.Domain.Entity.Reviews;
using GameService.DTO.Review;

namespace GameService.Mappers;

public static class ReviewMappers
{
    public static ReviewDto ToReviewDto(this Review reviewModel)
    {
        return new ReviewDto
        {
            Id = reviewModel.Id,
            Name = reviewModel.Name,
            Title = reviewModel.Title,
            Content = reviewModel.Content,
            CreatedOn = reviewModel.CreatedOn,
        };
    }

    public static (Review review, GameReview gameReview) ToReviewFromCreateDto(this CreateReviewDto dto, int gameId)
    {
        var review = new Review
        {
            Name = dto.Name,
            Title = dto.Title,
            Content = dto.Content,
            CreatedOn = dto.CreatedOn
        };

        var gameReview = new GameReview()
        {
            GameId = gameId,
            Review = review
        };

        return (review, gameReview);
    }
    
}