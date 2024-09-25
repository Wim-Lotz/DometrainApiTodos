namespace Todos.Contracts.Requests;

public record CreateTodoRequest
{
    public required string Description { get; init; }
}