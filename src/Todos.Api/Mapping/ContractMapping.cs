using Todos.Application.Models;
using Todos.Contracts.Requests;
using Todos.Contracts.Responses;

namespace Todos.Api.Mapping;

public static class ContractMapping
{
    public static Todo MapToTodo(this CreateTodoRequest request, Guid userId)
    {
        return new Todo
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Description = request.Description
        };
    }
    
    public static Todo MapToTodo(this UpdateTodoRequest request, Guid id, Guid userId)
    {
        return new Todo
        {
            Id = id,
            UserId = userId,
            Description = request.Description,
            CompletedOn = request.CompletedOn
        };
    }

    public static TodoResponse MapToResponse(this Todo todo)
    {
        return new TodoResponse
        {
            Id = todo.Id,
            UserId = todo.UserId,
            Description = todo.Description,
            CompletedOn = todo.CompletedOn,
            Completed = todo.Completed
        };
    }

    public static TodosResponse MapToResponse(this IEnumerable<Todo> todos, int page, int pageSize)
    {
        return new TodosResponse
        {
            Items = todos.Select(MapToResponse),
            Page = page,
            PageSize = pageSize
        };
    }

    public static GetAllTodosOptions MapToOptions(this GetAllTodosRequest request)
    {
        return new GetAllTodosOptions
        {
            Description = request.Description,
            SortField = request.SortBy?.Trim('+','-'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted:
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public static GetAllTodosOptions WithUser(this GetAllTodosOptions options, Guid? userId)
    {
         options.UserId = userId;
         return options;
    }
}