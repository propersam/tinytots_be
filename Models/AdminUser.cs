namespace Tinytots.Models;

public class AdminUser
{
    public int AdminUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // BCrypt hash
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
