using FluentValidation;

using Todos.Application.Models;
using Todos.Application.Repositories;
using Todos.Application.Validators;

namespace Todos.Application.Services;

public sealed class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;
    private readonly TodoValidator _todoValidator;

    public TodoService(ITodoRepository todoRepository, TodoValidator todoValidator)
    {
        _todoRepository = todoRepository;
        _todoValidator = todoValidator;
    }

    public async Task<bool> CreateAsync(Todo todo)
    {
        await _todoValidator.ValidateAndThrowAsync(todo);
        return await _todoRepository.CreateAsync(todo);
    }

    public Task<Todo?> GetByIdAsync(Guid id)
    {
        return _todoRepository.GetByIdAsync(id);
    }

    public Task<IEnumerable<Todo>> GetAllAsync()
    {
        return _todoRepository.GetAllAsync();
    }

    public async Task<Todo?> UpdateAsync(Todo todo)
    {
        await _todoValidator.ValidateAndThrowAsync(todo);
        
        var todoExists = await _todoRepository.ExistsByIdAsync(todo.Id);

        if (!todoExists)
            return null;
        await _todoRepository.UpdateAsync(todo);
        return todo;
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        return _todoRepository.DeleteByIdAsync(id);
    }
}