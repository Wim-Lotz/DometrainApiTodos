namespace Todos.Contracts.Requests;

public record UpdateTodoRequest
{
    public required string Description { get; init; }
}