using GameService.Application.Features.Games.DTO;
using GameService.Domain.Entity.Games;
using DTO_GameDto = GameService.Application.Features.Games.DTO.GameDto;
using GameDto = GameService.Application.Features.Games.DTO.GameDto;

namespace GameService.Mappers
{
    public static class GameMappers
    {
        public static DTO_GameDto ToGameDto(this Game gameModel)
        {
            var gameDto = new DTO_GameDto();
            gameDto.WithId(gameModel.Id);
            gameDto.WithTitle(gameModel.Title);
            gameDto.WithDescription(gameModel.Description);
            gameDto.WithPrice(gameModel.Price);
            gameDto.WithReviews(gameModel.GameReviews.Select(r=> r.Review.ToReviewDto()).ToList());
            return gameDto;
        }

        public static Game ToGameFromCreateDto(this CreateGameDto dto)
        {
            return new Game
            {
                Title = dto.Title,
                Description = dto.Description,
                ReleaseDate = dto.ReleaseDate,
                Price = dto.Price
            };
        }
    } 
}

