namespace Application.Request.Auth;

public record TokenRequest(string Email, string Password);