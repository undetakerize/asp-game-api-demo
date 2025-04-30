using GameService.DTO.Review;
using Lombok.NET;

namespace GameService.Application.Features.Games.DTO
{
    [With]
    public partial class GameDto
    {
        public int Id;
        public string Title;
        public string Description;
        public DateTime ReleaseDate;
        public decimal Price;
        public List<ReviewDto> Reviews;
    }
}

