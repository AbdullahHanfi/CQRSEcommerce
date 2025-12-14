
using Application.Abstractions.Services;
using Application.Builders;
using Application.DTOs.Auth;
using Application.Request.Auth;
using Domain.Shared.Results;

namespace Application.Features.Auth.Command.Login;

[TestFixture]
public class LoginCommandHandlerTests
{
    private Mock<IAuthenticationService> _mockAuthenticationService;
    private LoginCommandHandler _handler;
    [SetUp]
    public void Setup()
    {
        _mockAuthenticationService = new Mock<IAuthenticationService>();
    }
    [Test]
    public async Task Handle_ValidCredentials_Returns_SuccessResult()
    {
        // Arrange
        var request = new TokenRequest("mail", "password");
        var command = new LoginCommand("mail", "password");
        var expectedResult = new AuthDtoBuilder()
            .Build();
        _mockAuthenticationService
            .Setup(e => e.GetTokenAsync(request))
            .ReturnsAsync(Result.Success(expectedResult));

        _handler = new LoginCommandHandler(_mockAuthenticationService.Object);
        // Act 
        var actualResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(actualResult.IsSuccess);
        Assert.That(expectedResult, Is.EqualTo(actualResult.Value));
    }
    [Test]
    public async Task Handle_InvalidCredentials_Returns_FailureResult()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "wrong_pass");
        _mockAuthenticationService
            .Setup(x => x.GetTokenAsync(It.IsAny<TokenRequest>()))
            .ReturnsAsync(Result.Failure<AuthDto>(new Error("Invalid credentials")));

        _handler = new LoginCommandHandler(_mockAuthenticationService.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsFailure);
    }
}