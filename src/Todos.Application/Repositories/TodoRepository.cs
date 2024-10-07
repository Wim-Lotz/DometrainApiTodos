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
                                   insert into todos (id, userid, description)
                                   values (@Id, @UserId, @Description)
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
            UserId = t.UserId,
            Description = t.Description,
            CompletedOn = t.CompletedOn
        });
    }

    public async Task<IEnumerable<Todo>> GetAllMineAsync(GetAllTodosOptions options, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var orderClause = string.Empty;
        if (options.SortField is not null)
        {
            orderClause = $"""
            order by {options.SortField} {(options.SortOrder == SortOrder.Ascending ? "asc" : "desc")}
            """;
        }
        
        var result = await connection.QueryAsync<Todo>(
            new CommandDefinition($"""
                                   select * from todos where (userid = @userId) and 
                                   (@description is null or description like ( '%' || @description|| '%'))
                                   {orderClause} 
                                   limit @pageSize 
                                   offset @pageOffset
                                   """, new
            {
                userId = options.UserId, 
                description = options.Description,
                pageSize = options.PageSize,
                pageOffset =  (options.Page - 1) * options.PageSize
            }, cancellationToken: token));

        return result.Select(t => new Todo
        {
            Id = t.Id,
            UserId = t.UserId,
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