namespace Todos.Contracts.Requests;

public class GetAllTodosRequest : PagedRequest
{
    public required string? Description { get; init; }
    public required string? SortBy { get; init; }
}