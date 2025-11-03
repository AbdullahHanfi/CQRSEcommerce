namespace Application.DTOs.Auth;

public record AuthDto (string UserName, string Email, string Token, DateTime ExpiresOn,string RefreshToken ,DateTime RefreshTokenExpiration,bool IsAuthenticated = false);