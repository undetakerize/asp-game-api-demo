using ReviewDto = GameService.Application.Features.Reviews.DTO.ReviewDto;

namespace GameService.Application.Features.Games.DTO
{
    public class GameDto
    {
        public int Id;
        public string Title;
        public string Description;
        public DateTime ReleaseDate;
        public decimal Price;
        public List<ReviewDto> Reviews;
    }
}

