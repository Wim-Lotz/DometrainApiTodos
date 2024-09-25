namespace Todos.Contracts.Responses;

public record TodoResponse
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
}