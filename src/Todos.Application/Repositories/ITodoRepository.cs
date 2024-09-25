using Todos.Application.Models;

namespace Todos.Application.Repositories;

public interface ITodoRepository
{
    Task<bool> CreateAsync(Todo todo);
    
    Task<Todo?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<Todo>> GetAllAsync();
    
    Task<bool> UpdateAsync(Todo todo);
    
    Task<bool> DeleteByIdAsync(Guid id);
}