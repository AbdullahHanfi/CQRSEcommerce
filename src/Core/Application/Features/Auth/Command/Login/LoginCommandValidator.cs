using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Features.Auth.Command.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .Matches(@"^(?!.*\.\.\s+)([A-Za-z0-9]+([.-][A-Za-z0-9]+)*)@([A-Za-z0-9]+([.-][A-Za-z0-9]+)*)\.[A-Za-z]{2,63}$",RegexOptions.IgnoreCase)
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
