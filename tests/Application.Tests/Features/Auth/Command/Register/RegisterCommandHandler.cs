using Application.Abstractions.Messaging;
using Application.Abstractions.Services;

namespace Application.Features.Auth.Command.Register
{
    public class RegisterCommandHandler(IAuthenticationService authService) : ICommandHandler<RegisterCommand, AuthDto>
    {
        public async Task<Result<AuthDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await authService.RegisterAsync(new(request.Email, request.Password, request.ConfirmPassword, request.UserName));
        }
    }
}
