namespace Application.Abstractions.Services;

public interface IAuthenticationService
{
    Task<Result<AuthDto>> RegisterAsync(RegisterRequest request);
    Task<Result<AuthDto>> GetTokenAsync(TokenRequest request);
    Task<Result<AuthDto>> RefreshTokenAsync(string token);
    Task<Result> RevokeTokenAsync(string token);
}