using System.Text.RegularExpressions;
using Application.Common.Constants;

namespace Application.Features.Auth.Command.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmailRequired)
            .Matches(@"^(?!.*\.\.\s+)([A-Za-z0-9]+([.+_-][A-Za-z0-9]+)*)@([A-Za-z0-9]+([.-][A-Za-z0-9]+)*)\.[A-Za-z]{2,63}$", RegexOptions.IgnoreCase)
            .WithMessage(ValidationMessages.InvalidEmailFormat);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.PasswordRequired)
            .MinimumLength(6).WithMessage(ValidationMessages.PasswordTooShort);
    }
}
