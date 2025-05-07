using System.ComponentModel.DataAnnotations;
using GameService.Domain.Entity.GameReviews;

namespace GameService.Domain.Entity.Reviews
{
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

