namespace GameService.Helpers.Games;

public partial class SearchQueryGame
{
    public string? Title { get; set; } = null;
    public string? Description { get; set; } = null;
    public string? Sort { get; set; } = null;
    public string? Keywords { get; set; } = null;
    public int PageSize { get; set; } = 5;
    public int Page { get; set; } = 10;
}