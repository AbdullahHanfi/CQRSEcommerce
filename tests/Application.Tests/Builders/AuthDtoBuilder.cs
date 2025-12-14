using Application.DTOs.Auth;

namespace Application.Builders;

public class AuthDtoBuilder
{
    private string _userName = "testuser";
    private string _email = "test.user@example.com";
    private string _token = "jwt-token";
    private DateTime _expiresOn = DateTime.UtcNow.AddHours(1);
    private string _refreshToken = "refresh-token";
    private DateTime _refreshTokenExpiration = DateTime.UtcNow.AddDays(7);
    private bool _isAuthenticated = true;

    public AuthDtoBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public AuthDtoBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public AuthDtoBuilder WithToken(string token, DateTime expiresOn)
    {
        _token = token;
        _expiresOn = expiresOn;
        return this;
    }

    public AuthDtoBuilder WithRefreshToken(string refreshToken, DateTime refreshTokenExpiration)
    {
        _refreshToken = refreshToken;
        _refreshTokenExpiration = refreshTokenExpiration;
        return this;
    }

    public AuthDtoBuilder WithIsAuthenticated(bool isAuthenticated)
    {
        _isAuthenticated = isAuthenticated;
        return this;
    }

    public AuthDto Build()
    {
        return new AuthDto(
            _userName,
            _email,
            _token,
            _expiresOn,
            _refreshToken,
            _refreshTokenExpiration,
            _isAuthenticated
        );
    }
}