using Domain.Entities;
using Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data;
using Seed;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<User> DomainUsers { get; set; }
    public DbSet<Product> Products { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(b =>
        {
            b.HasOne(au => au.DomainUser)
             .WithOne()
             .HasForeignKey<ApplicationUser>(au => au.DomainUserId);
        });

        builder.Entity<User>().ToTable("Users");

        builder.Entity<User>(entity =>
        {
            // Configure unique index on Email , Username properties
            entity.HasIndex(e => e.Email).IsUnique();   
            entity.HasIndex(e => e.Username).IsUnique();

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(250);
            entity.Property(e => e.LastSeen).IsRequired();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(250);
        });

        builder.Entity<Product>(entity =>
        {
            // Configure unique index on ProductCode property
            entity.HasIndex(e => e.ProductCode).IsUnique();

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
            entity.Property(e => e.ProductCode).IsRequired().HasMaxLength(250);
            entity.Property(e => e.ImagePath).IsRequired().HasMaxLength(250);
            entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Category).IsRequired().HasMaxLength(250);
            
            // Seed initial data
            entity.HasData(Seed.Products());
        });

    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddUserToDomain();

        return await base.SaveChangesAsync(cancellationToken);
    }
    private void AddUserToDomain()
    {
        var newAppUsers = ChangeTracker.Entries<ApplicationUser>()
            .Where(e => e.State == EntityState.Added)
            .ToList();

        foreach (var entry in newAppUsers)
        {
            var appUser = entry.Entity;

            if (appUser.DomainUserId == Guid.Empty)
            {
                var domainUser = new User
                {
                    Username = appUser.UserName,
                    LastSeen = DateTime.UtcNow,
                    Email = appUser.Email,
                };

                DomainUsers.Add(domainUser);

                appUser.DomainUser = domainUser;
            }
        }
    }
}
