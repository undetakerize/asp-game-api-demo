using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameService.Domain.Entity.GameReviews;
using Lombok.NET;

namespace GameService.Domain.Entity.Games
{
    [With]
    public partial class Game
    {
        [Key]
        public int Id { get; set; }
        public string Title{ get;set;}
        public string Description{ get; set;}
        public DateTime ReleaseDate{ get; set;}
        [Column(TypeName = "decimal(18,2)")] public decimal Price { get; set;}
        public IList<GameReview> GameReviews { get; set; } = new List<GameReview>();
    } 
}

