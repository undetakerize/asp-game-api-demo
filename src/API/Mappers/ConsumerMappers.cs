using GameService.Application.Features.Reviews.DTO;

namespace GameService.Mappers;

public static class ConsumerMappers
{
    public static CreateReviewDto ToCreateReviewDto(this SyncConsumerReviewDto dto)
    {
        return new CreateReviewDto
        {
            Title = dto.Review.Title,
            Name = dto.Review.Name,
            Content = dto.Review.Content,
            CreatedOn = dto.Review.CreatedOn,
        };
    }
}