using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core;

public sealed class User : IdentityUser<Guid>
{
    [Key]
    public override Guid Id { get; set; } = Guid.NewGuid();
}