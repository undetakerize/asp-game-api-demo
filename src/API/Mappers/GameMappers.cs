using System.Globalization;
using GameService.Application.Features.Games.Command;
using GameService.Application.Features.Games.DTO;
using GameService.Domain.Entity.Games;
using DTO_GameDto = GameService.Application.Features.Games.DTO.GameDto;
using GameDto = GameService.Application.Features.Games.DTO.GameDto;

namespace GameService.Mappers
{
    public static class GameMappers
    {
        public static GameDto ToGameDto(this Game gameModel)
        {
            return new GameDto
            {
                Id = gameModel.Id,
                Title = gameModel.Title,
                Description = gameModel.Description,
                Price = gameModel.Price,
                Reviews = gameModel.GameReviews.Select(r => r.Review.ToReviewDto()).ToList()
            };
        }

        public static CommandCreateGame ToCommand(this CreateGameDto dto)
        {
            return new CommandCreateGame(
                dto.Title,
                dto.Description,
                dto.ReleaseDate,
                dto.Price
            );
        }
        
        public static CommandUpdateGame ToUpdateCommand(this UpdateGameDto dto, int id)
        {
            if (!DateTime.TryParseExact(dto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                throw new ArgumentException("Invalid date format. Use dd/MM/yyyy");

            parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            
            return new CommandUpdateGame(
                id,
                dto.Title,
                dto.Description,
                parsedDate,
                dto.Price
            );
        }
    } 
}

