using Todos.Application.Models;
using Todos.Contracts.Requests;
using Todos.Contracts.Responses;

namespace Todos.Api.Mapping;

public static class ContractMapping
{
    public static Todo MapToTodo(this CreateTodoRequest request)
    {
        return new Todo
        {
            Id = Guid.NewGuid(),
            Description = request.Description
        };
    }
    
    public static Todo MapToTodo(this UpdateTodoRequest request, Guid id)
    {
        return new Todo
        {
            Id = id,
            Description = request.Description,
            CompletedOn = request.CompletedOn
        };
    }

    public static TodoResponse MapToResponse(this Todo todo)
    {
        return new TodoResponse
        {
            Id = todo.Id,
            Description = todo.Description,
            CompletedOn = todo.CompletedOn,
            Completed = todo.Completed
        };
    }

    public static TodosResponse MapToResponse(this IEnumerable<Todo> todos)
    {
        return new TodosResponse
        {
            Items = todos.Select(MapToResponse)
        };
    }
}