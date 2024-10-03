using FluentValidation;

using Todos.Application.Models;
using Todos.Application.Repositories;
using Todos.Application.Validators;

namespace Todos.Application.Services;

public sealed class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;
    private readonly TodoValidator _todoValidator;
    private readonly GetAllTodosOptionsValidator _optionsValidator;

    public TodoService(ITodoRepository todoRepository, TodoValidator todoValidator, GetAllTodosOptionsValidator optionsValidator)
    {
        _todoRepository = todoRepository;
        _todoValidator = todoValidator;
        _optionsValidator = optionsValidator;
    }

    public async Task<bool> CreateAsync(Todo todo, CancellationToken token = default)
    {
        await _todoValidator.ValidateAndThrowAsync(todo, cancellationToken: token);
        return await _todoRepository.CreateAsync(todo, token);
    }

    public Task<Todo?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _todoRepository.GetByIdAsync(id, token);
    }

    public Task<IEnumerable<Todo>> GetAllAsync(CancellationToken token = default)
    {
        return _todoRepository.GetAllAsync(token);
    }

    public async Task<IEnumerable<Todo>> GetAllMineAsync(GetAllTodosOptions options, CancellationToken token = default)
    {
        await _optionsValidator.ValidateAndThrowAsync(options, cancellationToken: token);
        
        return await _todoRepository.GetAllMineAsync(options, token);
    }

    public async Task<Todo?> UpdateAsync(Todo todo, CancellationToken token = default)
    {
        await _todoValidator.ValidateAndThrowAsync(todo, cancellationToken: token);
        
        var todoExists = await _todoRepository.ExistsByIdAsync(todo.Id, token);

        if (!todoExists)
            return null;
        await _todoRepository.UpdateAsync(todo, token);
        return todo;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _todoRepository.DeleteByIdAsync(id, token);
    }
}