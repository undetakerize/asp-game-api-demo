using Lombok.NET;

namespace GameService.Helpers.Games;

[With]
public partial class SearchQueryGame
{
    public string? Title { get; set; } = null;
    public string? Description { get; set; } = null;
    public string? Sort { get; set; } = null;
    public string? Keywords { get; set; } = null;
}