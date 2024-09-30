using Todos.Application.Models;

namespace Todos.Application.Services;

public interface ITodoService
{
    Task<bool> CreateAsync(Todo todo);
    
    Task<Todo?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<Todo>> GetAllAsync();
    
    Task<Todo?> UpdateAsync(Todo todo);
    
    Task<bool> DeleteByIdAsync(Guid id);
}