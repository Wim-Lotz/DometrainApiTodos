using Todos.Application.Models;

namespace Todos.Application.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly List<Todo> _todos = [];
    
    public Task<bool> CreateAsync(Todo todo)
    {
        _todos.Add(todo);
        return Task.FromResult(true);
    }

    public Task<Todo?> GetByIdAsync(Guid id)
    {
        var todo = _todos.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(todo);
    }

    public Task<IEnumerable<Todo>> GetAllAsync()
    {
        return Task.FromResult(_todos.AsEnumerable());
    }

    public Task<bool> UpdateAsync(Todo todo)
    {
        var todoIndex = _todos.FindIndex(x => x.Id == todo.Id);
        if (todoIndex == -1)
        {
            return Task.FromResult(false);
        }

        _todos[todoIndex] = todo;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removedCount = _todos.RemoveAll(x => x.Id == id);
        var todoRemoved = removedCount > 0; 
        return Task.FromResult(todoRemoved);
    }
}