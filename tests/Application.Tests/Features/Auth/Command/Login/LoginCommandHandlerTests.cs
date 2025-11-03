namespace Application.Tests.Features.Auth.Command.Login;

[TestFixture]
public class LoginCommandHandlerTests
{
    private Mock<IAuthenticationService> _authServiceMock;
    private LoginCommandHandler _handler;
    private CancellationToken _cancellationToken;

    [SetUp]
    public void SetUp()
    {
        _authServiceMock = new Mock<IAuthenticationService>();
        _handler = new LoginCommandHandler(_authServiceMock.Object);
        _cancellationToken = CancellationToken.None;
    }

    [Test]
    public async Task Handle_ValidCredentials_ReturnsSuccessfulAuthDto()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Password123");
        var expectedAuthDto = new AuthDto(
            "TestUser",
            "test@example.com",
            "jwt-token-here",
            DateTime.UtcNow.AddHours(1),
            "refresh-token-here",
            DateTime.UtcNow.AddDays(7),
            true
        );

        _authServiceMock
            .Setup(x => x.GetTokenAsync(It.IsAny<TokenRequest>()))
            .ReturnsAsync(Result.Success(expectedAuthDto));

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Email, Is.EqualTo(expectedAuthDto.Email));
        Assert.That(result.Value.UserName, Is.EqualTo(expectedAuthDto.UserName));
        Assert.That(result.Value.Token, Is.EqualTo(expectedAuthDto.Token));
        Assert.That(result.Value.IsAuthenticated, Is.True);
    }

    [Test]
    public async Task Handle_InvalidCredentials_ReturnsFailureResult()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "WrongPassword");
        var error = new Error("Email or Password is incorrect!");

        _authServiceMock
            .Setup(x => x.GetTokenAsync(It.IsAny<TokenRequest>()))
            .ReturnsAsync(Result.Failure<AuthDto>(error));

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error.Message, Is.EqualTo("Email or Password is incorrect!"));
    }

    [Test]
    public async Task Handle_CallsAuthServiceWithCorrectParameters()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Password123");
        TokenRequest capturedRequest = null;

        _authServiceMock
            .Setup(x => x.GetTokenAsync(It.IsAny<TokenRequest>()))
            .Callback<TokenRequest>(req => capturedRequest = req)
            .ReturnsAsync(Result.Success(CreateAuthDto()));

        // Act
        await _handler.Handle(command, _cancellationToken);

        // Assert
        _authServiceMock.Verify(x => x.GetTokenAsync(It.IsAny<TokenRequest>()), Times.Once);
        Assert.That(capturedRequest, Is.Not.Null);
        Assert.That(capturedRequest.Email, Is.EqualTo(command.Email));
        Assert.That(capturedRequest.Password, Is.EqualTo(command.Password));
    }

    [Test]
    public async Task Handle_WhenAuthServiceThrowsException_ThrowsException()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Password123");
        var exceptionMessage = "Database connection failed";

        _authServiceMock
            .Setup(x => x.GetTokenAsync(It.IsAny<TokenRequest>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await _handler.Handle(command, _cancellationToken));

        Assert.That(ex.Message, Is.EqualTo(exceptionMessage));
    }

    private AuthDto CreateAuthDto()
    {
        return new AuthDto(
            "TestUser",
            "test@example.com",
            "jwt-token",
            DateTime.UtcNow.AddHours(1),
            "refresh-token",
            DateTime.UtcNow.AddDays(7),
            true
        );
    }
}
