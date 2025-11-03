using Application.Abstractions.Messaging;
using Application.Abstractions.Services;

namespace Application.Features.Auth.Command.RefreshToken;
public class RefreshTokenCommandHandler(IAuthenticationService authService) : ICommandHandler<RefreshTokenCommand, AuthDto>
{
    public async Task<Result<AuthDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.RefreshTokenAsync(request.Token);
        return result;
    }
}
