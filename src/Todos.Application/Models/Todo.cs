namespace Todos.Application.Models;

public sealed class Todo
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
}