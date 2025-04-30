using System.ComponentModel.DataAnnotations;
using Lombok.NET;

namespace GameService.Application.Features.Games.DTO;

[With]
[AllArgsConstructor]
public partial class UpdateGameDto
{
    public string Title;
    [MaxLength(100, ErrorMessage = "Description cannot max than {1} characters")]
    public string Description;
    public DateTime ReleaseDate;
    public decimal Price;
}