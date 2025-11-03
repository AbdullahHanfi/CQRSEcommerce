namespace Application.Features.Auth.Command.Login;

using Abstractions.Messaging;
using Application.Abstractions.Services;

public class LoginCommandHandler(IAuthenticationService authService) : ICommandHandler<LoginCommand, AuthDto>
{
    public async Task<Result<AuthDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.GetTokenAsync(new TokenRequest(request.Email, request.Password));
        return result;
    }
}
