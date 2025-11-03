namespace Application.Features.Auth.Command.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Your Email cannot be empty.")
            .EmailAddress().WithMessage("Your Email must be valid Email.");

        RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Your password cannot be empty.")
                    .MinimumLength(6).WithMessage("Your password must be at least 6 characters long.");

        RuleFor(r => r.UserName)
            .NotEmpty().WithMessage("Your Username cannot be empty.");

        RuleFor(x => x.ConfirmPassword)
           .NotEmpty().WithMessage("Please confirm your password.")
           .Equal(x => x.Password).WithMessage("The passwords do not match.");
    }
}
