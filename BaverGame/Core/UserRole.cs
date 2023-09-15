using System.ComponentModel.DataAnnotations;

namespace Core;

public class UserRole
{
    [Key]
    public Guid RoleId { get; } = Guid.NewGuid();
    [StringLength(255)]
    public string RoleName { get; set; }
    public ICollection<User> Users { get; set; }
}