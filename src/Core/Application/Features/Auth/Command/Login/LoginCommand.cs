using Application.Abstractions.Messaging;

namespace Application.Features.Auth.Command.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthDto>;