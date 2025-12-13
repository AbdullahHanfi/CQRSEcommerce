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
            .NotEmpty().WithMessage(ValidationMessages.UserNameRequired);

        RuleFor(x => x.ConfirmPassword)
           .NotEmpty().WithMessage(ValidationMessages.ConfirmPasswordRequired)
           .Equal(x => x.Password).WithMessage(ValidationMessages.ConfirmPasswordNotMatch);
    }
}
