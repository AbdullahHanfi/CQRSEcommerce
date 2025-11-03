
namespace Application.Tests.Features.Auth.Command.Login;

[TestFixture]
public class LoginCommandValidatorTests
{
    private LoginCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new LoginCommandValidator();
    }

    [Test]
    public void Validate_ValidCommand_PassesValidation()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Password123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    [TestCase("", "Password123", "Email is required.")]
    [TestCase("invalid-email", "Password123", "Invalid email format.")]
    [TestCase("@example.com", "Password123", "Invalid email format.")]
    public void Validate_InvalidEmail_FailsValidation(string email, string password, string expectedError)
    {
        // Arrange
        var command = new LoginCommand(email, password);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(x => x.ErrorMessage == expectedError), Is.True);
    }

    [Test]
    [TestCase("test@example.com", "", "Password is required.")]
    [TestCase("test@example.com", "12345", "Password must be at least 6 characters long.")]
    public void Validate_InvalidPassword_FailsValidation(string email, string password, string expectedError)
    {
        // Arrange
        var command = new LoginCommand(email, password);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(x => x.ErrorMessage == expectedError), Is.True);
    }

    [Test]
    public void Validate_BothFieldsInvalid_ReturnsTwoErrors()
    {
        // Arrange
        var command = new LoginCommand("", "123123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(2));
    }
    [Test]
    public void Validate_BothFieldsInvalid_ReturnsThreeErrors()
    {
        // Arrange
        var command = new LoginCommand("", "12312");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(3));
    }
}