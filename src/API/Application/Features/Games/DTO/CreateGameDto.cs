using System.ComponentModel.DataAnnotations;

namespace GameService.Application.Features.Games.DTO;

public class CreateGameDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ReleaseDate { get; set; }
    public decimal Price { get; set; }
}