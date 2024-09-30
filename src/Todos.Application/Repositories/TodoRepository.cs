using Dapper;
using Todos.Application.Database;
using Todos.Application.Models;

namespace Todos.Application.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TodoRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Todo todo, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var result = await connection.ExecuteAsync(
            new CommandDefinition($"""
                                   insert into todos (id, description)
                                   values (@Id, @Description)
                                   """, todo, cancellationToken: token));

        return result > 0;
    }

    public async Task<Todo?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var todo = await connection.QuerySingleOrDefaultAsync<Todo>(
            new CommandDefinition($"""
                                   select * from todos where id = @id
                                   """, new { id }, cancellationToken: token));
        return todo ?? null;
    }

    public async Task<IEnumerable<Todo>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        
        var result = await connection.QueryAsync<Todo>(
            new CommandDefinition($"""
                                   select * from todos
                                   """, cancellationToken: token));

        return result.Select(t => new Todo
        {
            Id = t.Id,
            Description = t.Description,
            CompletedOn = t.CompletedOn
        });
    }

    public async Task<bool> UpdateAsync(Todo todo, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var result = await connection.ExecuteAsync(
            new CommandDefinition($"""
                                   update todos set description = @Description, completedOn = @CompletedOn, completed = @Completed
                                   where id = @Id
                                   """, todo, cancellationToken: token));
        
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        
        var result = await connection.ExecuteAsync(
            new CommandDefinition($"""
                                   delete from todos where id = @id
                                   """, new { id }, cancellationToken: token));
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition($"""
                                   select count(1) from todos where id = @id
                                   """, new { id }, cancellationToken: token));
    }
}