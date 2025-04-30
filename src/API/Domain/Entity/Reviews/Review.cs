using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameService.Domain.Entity.GameReviews;
using Lombok.NET;

namespace GameService.Domain.Entity.Reviews
{
    [With]
    public partial class Review
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public IList<GameReview> GameReviews { get; set; } = new List<GameReview>();
    }   
}

