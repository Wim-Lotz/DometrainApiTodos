using FluentValidation;

using Todos.Application.Models;

namespace Todos.Application.Validators;

public class TodoValidator : AbstractValidator<Todo>
{
    public TodoValidator()
    {
        RuleFor(t => t.Id).NotEmpty();
        RuleFor(t => t.Description).NotEmpty();
    }
}