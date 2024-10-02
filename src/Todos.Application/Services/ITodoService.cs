using Todos.Application.Models;

namespace Todos.Application.Services;

public interface ITodoService
{
    Task<bool> CreateAsync(Todo todo, CancellationToken token = default);
    
    Task<Todo?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<IEnumerable<Todo>> GetAllAsync(CancellationToken token = default);
    
    Task<IEnumerable<Todo>> GetAllMineAsync(Guid userId = default, CancellationToken token = default);
    
    Task<Todo?> UpdateAsync(Todo todo, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}