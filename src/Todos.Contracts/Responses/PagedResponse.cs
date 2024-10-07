namespace Todos.Contracts.Responses;

public class PagedResponse<TResponse>
{
    public IEnumerable<TResponse> Items { get; init; } = [];

    public int Page { get; set; }
    public int PageSize { get; set; }
}