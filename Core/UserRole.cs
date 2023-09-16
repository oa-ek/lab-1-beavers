using System.ComponentModel.DataAnnotations;

namespace Core;

public sealed class UserRole
{
    [Key]
    public Guid RoleId { get; } = Guid.NewGuid();
    [MaxLength(255)]
    public string RoleName { get; set; }
    public ICollection<User> Users { get; set; }
}