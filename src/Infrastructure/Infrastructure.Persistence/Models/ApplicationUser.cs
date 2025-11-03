using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid DomainUserId { get; set; }
    public User DomainUser { get; set; } = null!;
    public List<RefreshToken>? RefreshTokens { get; set; } = new();
    public DateTime RefreshTokenExpiryTime { get; set; }
}
