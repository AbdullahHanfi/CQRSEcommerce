using Application.Abstractions.Services;
using Application.BuilderEntities;
using Application.DTOs.Auth;
using Application.Request.Auth;
using Domain.Shared.Results;

namespace Application.Features.Auth.Command.Register;

[TestFixture]
public class RegisterCommandHandlerTests
{
    private Mock<IAuthenticationService> _mockAuthenticationService;
    private RegisterCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockAuthenticationService = new Mock<IAuthenticationService>();
    }

    [Test]
    public async Task Handle_WithValidDetails_ShouldReturnSuccessResultWithAuthDto()
    {
        // Arrange
        var command = new RegisterCommandBuilder()
            .Build();
        var expectedAuthDto = new AuthDtoBuilder()
            .Build();

        _mockAuthenticationService
            .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequest>()))
            .ReturnsAsync(Result.Success(expectedAuthDto));

        _handler = new RegisterCommandHandler(_mockAuthenticationService.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess);
        Assert.That(expectedAuthDto, Is.EqualTo(result.Value));
        _mockAuthenticationService
            .Verify(s => s.RegisterAsync(It.IsAny<RegisterRequest>()), Times.Once);
    }

    [Test]
    public async Task Handle_WithDuplicateEmail_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new RegisterCommandBuilder().WithEmail("existing.user@example.com").Build();
        var error = new Error("User with this email already exists.");

        _mockAuthenticationService
            .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequest>()))
            .ReturnsAsync(Result.Failure<AuthDto>(error));
        _handler = new RegisterCommandHandler(_mockAuthenticationService.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.EqualTo(error));
    }

    [Test]
    public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var command = new RegisterCommandBuilder().Build();
        var exception = new System.Exception("Service failure");

        _mockAuthenticationService
            .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequest>()))
            .ThrowsAsync(exception);
        _handler = new RegisterCommandHandler(_mockAuthenticationService.Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(command, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("Service failure"));
    }
}