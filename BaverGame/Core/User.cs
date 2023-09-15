namespace Core;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public Guid UserId { get; } = Guid.NewGuid();
    [StringLength(255)]
    public string Username { get; set; }
    [StringLength(255)]
    public string Password { get; set; }
    [StringLength(255)]
    public string Email { get; set; }
    public Guid? RoleId { get; set; }
    public UserRole Role { get; set; }
    public ICollection<UserGameOwnership> UserGameOwnerships { get; set; }
}