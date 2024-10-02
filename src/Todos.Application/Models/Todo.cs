namespace Todos.Application.Models;

public sealed class Todo
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
    public DateTime? CompletedOn { get; init; }
    public bool Completed => IsComplete();
    public Guid UserId { get; init; }

    private bool IsComplete()
    {
        return CompletedOn.HasValue;
    }
}