using Microsoft.Extensions.DependencyInjection;
using Todos.Application.Repositories;

namespace Todos.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITodoRepository, TodoRepository>();
        return services;
    }
}