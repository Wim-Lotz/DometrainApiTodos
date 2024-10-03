using FluentValidation;

using Todos.Application.Models;

namespace Todos.Application.Validators;

public class GetAllTodosOptionsValidator : AbstractValidator<GetAllTodosOptions>
{
    private static readonly string[] AllowedFields = ["description"];
    
    public GetAllTodosOptionsValidator()
    {
        RuleFor(o => o.SortField)
            .Must(x => x is null || AllowedFields.Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Bad SortField bro");
    }
}