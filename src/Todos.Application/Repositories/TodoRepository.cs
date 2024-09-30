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

    public async Task<bool> CreateAsync(Todo todo)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var result = await connection.ExecuteAsync(
            new CommandDefinition($"""
                                   insert into todos (id, description)
                                   values (@Id, @Description)
                                   """, todo));

        return result > 0;
    }

    public async Task<Todo?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var todo = await connection.QuerySingleOrDefaultAsync<Todo>(
            new CommandDefinition($"""
                                   select * from todos where id = @id
                                   """, new { id }));
        return todo ?? null;
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        var result = await connection.QueryAsync<Todo>(
            new CommandDefinition($"""
                                   select * from todos
                                   """));

        return result.Select(t => new Todo
        {
            Id = t.Id,
            Description = t.Description,
            CompletedOn = t.CompletedOn
        });
    }

    public async Task<bool> UpdateAsync(Todo todo)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var result = await connection.ExecuteAsync(
            new CommandDefinition($"""
                                   update todos set description = @Description, completedOn = @CompletedOn, completed = @Completed
                                   where id = @Id
                                   """, todo));
        
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        var result = await connection.ExecuteAsync(
            new CommandDefinition($"""
                                   delete from todos where id = @id
                                   """, new { id }));
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition($"""
                                   select count(1) from todos where id = @id
                                   """, new { id }));
    }
}