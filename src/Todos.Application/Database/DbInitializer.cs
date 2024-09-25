using Dapper;

namespace Todos.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync($"""
                                       create table if not exists todos (
                                           id UUID primary key,
                                           description text not null,
                                           completedOn timestamp null,
                                           completed boolean not null default false);
                                       """);
    }
}