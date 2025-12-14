using System.Text.RegularExpressions;
using Application.Common.Constants;

namespace Application.Features.Auth.Command.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmailRequired)
            .Matches(@"^(?!.*\.\.\s+)([A-Za-z0-9]+([.+_-][A-Za-z0-9]+)*)@([A-Za-z0-9]+([.-][A-Za-z0-9]+)*)\.[A-Za-z]{2,63}$", RegexOptions.IgnoreCase)
            .WithMessage(ValidationMessages.InvalidEmailFormat);

        RuleFor(x => x.Password)
                    .NotEmpty().WithMessage(ValidationMessages.PasswordRequired)
                    .MinimumLength(6).WithMessage(ValidationMessages.PasswordTooShort);

        RuleFor(r => r.UserName)
            .NotEmpty().WithMessage(ValidationMessages.UsernameRequired)
            .MaximumLength(20)
            .WithMessage(ValidationMessages.UsernameLength)
            .Matches(@"^[a-zA-Z][a-zA-Z0-9_]{1,20}$")
            .WithMessage(ValidationMessages.UsernameInvalidCharacters);

        RuleFor(x => x.ConfirmPassword)
           .NotEmpty().WithMessage(ValidationMessages.ConfirmPasswordRequired)
           .Equal(x => x.Password).WithMessage(ValidationMessages.ConfirmPasswordNotMatch);
    }
}
