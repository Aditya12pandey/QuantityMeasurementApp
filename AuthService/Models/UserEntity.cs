using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models;

[Table("users")]
public class UserEntity
{
    [Key]
    [Column("user_id")]
    [MaxLength(64)]
    public string UserId { get; set; }

    [Required]
    [Column("username")]
    [MaxLength(100)]
    public string Username { get; set; }

    [Required]
    [Column("password_hash")]
    [MaxLength(256)]
    public string PasswordHash { get; set; }

    [Required]
    [Column("role")]
    [MaxLength(32)]
    public string Role { get; set; } = "User";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public UserEntity() { }

    public UserEntity(string username, string passwordHash, string role = "User")
    {
        UserId       = Guid.NewGuid().ToString();
        Username     = username;
        PasswordHash = passwordHash;
        Role         = role;
        CreatedAt    = DateTime.UtcNow;
    }
}
