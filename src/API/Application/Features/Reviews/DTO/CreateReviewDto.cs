using System.ComponentModel.DataAnnotations;
using Lombok.NET;

namespace GameService.DTO.Review;
[With]
public partial class CreateReviewDto
{
    [Required]
    public string Name {get; set;}
    [Required]
    [MinLength(1, ErrorMessage = "Title Must be {1} characters")]
    [MaxLength(50, ErrorMessage = "Title cannot max than {1} characters")]
    public string Title { get; set; } = String.Empty;
    [MaxLength(250, ErrorMessage = "Content cannot max than {1} characters")]
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}