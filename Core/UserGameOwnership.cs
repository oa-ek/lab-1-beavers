using System.ComponentModel.DataAnnotations;

namespace Core;

public sealed class UserGameOwnership
{
    [Key]
    public Guid OwnershipId { get; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public bool Owned { get; set; }
    public User User { get; set; }
    public Game Game { get; set; }
}