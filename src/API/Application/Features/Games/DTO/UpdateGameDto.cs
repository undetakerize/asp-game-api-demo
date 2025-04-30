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
    [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "Date must be in format dd/MM/yyyy")]
    public string ReleaseDate;
    public decimal Price;
}