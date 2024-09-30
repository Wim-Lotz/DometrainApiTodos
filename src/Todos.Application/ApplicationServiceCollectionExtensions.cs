using FluentValidation;

using Microsoft.Extensions.DependencyInjection;
using Todos.Application.Database;
using Todos.Application.Repositories;
using Todos.Application.Services;

namespace Todos.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITodoRepository, TodoRepository>();
        services.AddSingleton<ITodoService, TodoService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}