using Application.Abstractions.Messaging;

namespace Application.Features.Auth.Command.Register;

public record RegisterCommand(string Email, string Password, string ConfirmPassword, string UserName) : ICommand<AuthDto>;