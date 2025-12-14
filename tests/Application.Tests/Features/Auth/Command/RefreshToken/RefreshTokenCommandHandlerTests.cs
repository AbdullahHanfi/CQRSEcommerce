using Application.Abstractions.Services;
using Application.Builders;
using Application.DTOs.Auth;
using Application.Features.Auth.Command.Login;
using Domain.Shared.Results;

namespace Application.Features.Auth.Command.RefreshToken;

[TestFixture]
public class RefreshTokenCommandHandlerTests
{
    private Mock<IAuthenticationService> _mockAuthenticationService;
    private RefreshTokenCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockAuthenticationService = new Mock<IAuthenticationService>();
    }

    [Test]
    public async Task Handle_ValidToken_Returns_SuccessResult()
    {
        // Arrange
        const string request = "refreshToken";
        var command = new RefreshTokenCommand(request);

        var expectedResult = new AuthDtoBuilder()
            .Build();

        _mockAuthenticationService
            .Setup(e => e.RefreshTokenAsync(request))
            .ReturnsAsync(Result.Success(expectedResult));

        _handler = new RefreshTokenCommandHandler(_mockAuthenticationService.Object);
        // Act 
        var actualResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(actualResult.IsSuccess);
        Assert.That(expectedResult, Is.EqualTo(actualResult.Value));
    }
    [Test]
    public async Task Handle_InvalidToken_Returns_FailureResult()
    {
        // Arrange
        var command = new RefreshTokenCommand("string");

        _mockAuthenticationService
            .Setup(x => x.RefreshTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(Result.Failure<AuthDto>(new Error("Invalid token")));
        _handler = new RefreshTokenCommandHandler(_mockAuthenticationService.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsFailure);
    }
}