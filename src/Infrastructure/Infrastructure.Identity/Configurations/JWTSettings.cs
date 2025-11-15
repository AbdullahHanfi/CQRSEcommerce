namespace Infrastructure.Identity.Configurations;

public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public double DurationInDays { get; set; }
    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Key))
            throw new InvalidOperationException("JWT Key is not configured");
            
        if (Key.Length < 32)
            throw new InvalidOperationException("JWT Key must be at least 32 characters long for security");
            
        // Prevent using default/weak keys
        if (Key == "fcecb4afff94c3159809527c8c2ccfd6" || Key.Length < 32)
            throw new InvalidOperationException("Insecure JWT Key detected. Use a strong, randomly generated key.");
            
        if (string.IsNullOrWhiteSpace(Issuer))
            throw new InvalidOperationException("JWT Issuer is not configured");
            
        if (string.IsNullOrWhiteSpace(Audience))
            throw new InvalidOperationException("JWT Audience is not configured");
    }
}