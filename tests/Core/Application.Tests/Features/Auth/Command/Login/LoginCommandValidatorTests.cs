using Application.Common.Constants;

namespace Application.Features.Auth.Command.Login;

[TestFixture]
public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Theory]
    [TestCaseSource(typeof(TestEmailConstants), nameof(TestEmailConstants.EmptyEmailCases))]
    public void Email_WhenEmpty_Should_HaveValidationError(string email)
    {
        // Arrange
        var command = new LoginCommand(email, "password");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(ValidationMessages.EmailRequired);
    }

    [Theory]
    [TestCaseSource(typeof(TestEmailConstants), nameof(TestEmailConstants.InvalidEmailCases))]
    public void Email_WhenInValid_Should_HaveValidationError(string email)
    {
        // Arrange
        var command = new LoginCommand(email, "password");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(ValidationMessages.InvalidEmailFormat);
    }
    [Theory]
    [TestCaseSource(typeof(TestEmailConstants), nameof(TestEmailConstants.ValidEmailCases))]
    public void Email_WhenIsValid_ShouldNot_HaveValidationError(string email)
    {
        // Arrange
        var command = new LoginCommand(email, "password");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}