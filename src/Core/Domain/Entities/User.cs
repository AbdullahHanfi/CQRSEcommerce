namespace Domain.Entities;


public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    // LastLoginTime
    public DateTime LastSeen { get; set; }
}