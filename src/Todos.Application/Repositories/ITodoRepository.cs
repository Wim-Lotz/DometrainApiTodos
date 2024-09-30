using Todos.Application.Models;

namespace Todos.Application.Repositories;

public interface ITodoRepository
{
    Task<bool> CreateAsync(Todo todo, CancellationToken token = default);
    
    Task<Todo?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<IEnumerable<Todo>> GetAllAsync(CancellationToken token = default);
    
    Task<bool> UpdateAsync(Todo todo, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
}