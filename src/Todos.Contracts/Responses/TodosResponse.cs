namespace Todos.Contracts.Responses;

public record TodosResponse
{
    public IEnumerable<TodoResponse> Items { get; init; } = [];
}