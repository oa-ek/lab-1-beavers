using System.ComponentModel.DataAnnotations;

namespace Core;

public sealed class User
{
    [Key]
    public Guid UserId { get; } = Guid.NewGuid();
    [MaxLength(255)]
    public string Username { get; set; }
    [MaxLength(255)]
    public string Password { get; set; }
    public string Email { get; set; }
    
    public Guid UserRoleId { get; set; }
    public UserRole UserRole { get; set; }
}