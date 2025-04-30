using System.ComponentModel.DataAnnotations;

namespace GameService.Application.Features.Games.DTO;

public class CreateGameDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Title cannot max than {1} characters")]
    public string Title { get; set; } = string.Empty;
    [MaxLength(100, ErrorMessage = "Description cannot max than {1} characters")]
    public string Description { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    [Range(0.001, 100, ErrorMessage = "Price cannot negative")]
    public decimal Price { get; set; }
}