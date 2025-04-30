using Lombok.NET;

namespace GameService.DTO.Review;

[With]
public partial class ReviewDto
{
    public int Id { get; set; }
    public string Name {get; set;}
    public string Title { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}