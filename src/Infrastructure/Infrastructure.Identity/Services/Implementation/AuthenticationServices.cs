namespace Infrastructure.Identity.Services.Implementation;

using Application.Abstractions.Services;
using Application.Request.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

public class AuthenticationService(UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jwt) : IAuthenticationService
{
    private readonly JWTSettings _jwt = jwt.Value;
    public async Task<Result<AuthDto>> RegisterAsync(RegisterRequest model)
    {
        if (await userManager.FindByEmailAsync(model.Email) is not null)
            return Result.Failure<AuthDto>(new("Email is already registered!"));

        if (await userManager.FindByNameAsync(model.UserName) is not null)
            return Result.Failure<AuthDto>(new("Username is already registered!"));

        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return Result.Failure<AuthDto>(new(errors));
        }

        var jwtSecurityToken = await CreateJwtToken(user);

        var refreshToken = GenerateRefreshToken();
        user.RefreshTokens?.Add(refreshToken);
        await userManager.UpdateAsync(user);

        return new AuthDto(user.UserName, user.Email, new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), jwtSecurityToken.ValidTo, refreshToken.Token, refreshToken.ExpiresOn, true);
    }

    public async Task<Result<AuthDto>> GetTokenAsync(TokenRequest model)
    {

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, model.Password)) { return Result.Failure<AuthDto>(new("Email or Password is incorrect!")); }

        var jwtSecurityToken = await CreateJwtToken(user);
        RefreshToken refreshToken;

        if (user.RefreshTokens.Any(t => t.IsActive)) { refreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive); }
        else
        {
            refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);
        }
        return new AuthDto(user.UserName!, user.Email!, new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), jwtSecurityToken.ValidTo, refreshToken.Token, refreshToken.ExpiresOn, true);
    }
    public async Task<Result<AuthDto>> RefreshTokenAsync(string token)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null) { return Result.Failure<AuthDto>(new("Invalid token")); }

        var refreshToken = user?.RefreshTokens?.Single(t => t.Token == token);

        if (!refreshToken?.IsActive ?? true) { return Result.Failure<AuthDto>(new("Inactive token")); }

        refreshToken.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await userManager.UpdateAsync(user);

        var jwtToken = await CreateJwtToken(user);

        return new AuthDto(user.UserName!, user.Email!, new JwtSecurityTokenHandler().WriteToken(jwtToken), jwtToken.ValidTo, refreshToken.Token, refreshToken.ExpiresOn, true);
    }

    public async Task<Result> RevokeTokenAsync(string token)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
            return Result.Failure(new("Invalid token"));

        var refreshToken = user?.RefreshTokens?.Single(t => t.Token == token);

        if (!refreshToken?.IsActive ?? true)
            return Result.Failure(new("Invalid token"));

        refreshToken.RevokedOn = DateTime.UtcNow;

        await userManager.UpdateAsync(user);

        return Result.Success();
    }


    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = RandomNumberGenerator.Create();

        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow
        };
    }
    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        await updateUserActivity(user);
        var userClaims = await userManager.GetClaimsAsync(user);

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("guid", user.Id.ToString())
            }
            .Union(userClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
        issuer: _jwt.Issuer,
        audience: _jwt.Audience,
        claims: claims,
        expires: DateTime.Now.AddDays(_jwt.DurationInDays),
        signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
    private async Task updateUserActivity(ApplicationUser user)
    {
        var updateUser = userManager.Users.Include(e => e.DomainUser).FirstOrDefault(e => e.Id == user.Id);

        updateUser!.DomainUser.LastSeen = DateTime.UtcNow;

        await userManager.UpdateAsync(updateUser);
    }
}