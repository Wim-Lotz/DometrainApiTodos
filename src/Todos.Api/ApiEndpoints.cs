namespace Todos.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    
    public static class Todos
    {
        private const string Base = $"{ApiBase}/todos";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
        
        public const string GetMyTodos = $"{Base}/me";
    }
        
}