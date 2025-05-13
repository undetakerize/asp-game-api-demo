namespace GameService.Application.Common;

public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
}