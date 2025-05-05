using System.ComponentModel.DataAnnotations;

namespace GameService.Application.Features.Reviews.DTO;

public class UpdateReviewDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Title Must be {1} characters")]
    [MaxLength(50, ErrorMessage = "Title cannot max than {1} characters")]
    public string Title { get; set; } = String.Empty;
    [MaxLength(250, ErrorMessage = "Content cannot max than {1} characters")]
    public string Content { get; set; } = String.Empty;
}