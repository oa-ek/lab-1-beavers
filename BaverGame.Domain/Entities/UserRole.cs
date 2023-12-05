using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BaverGame.Domain.Entities;

public sealed class UserRole : IdentityRole<Guid>
{
    [Key]
    public override Guid Id { get; set; } = Guid.NewGuid();
}