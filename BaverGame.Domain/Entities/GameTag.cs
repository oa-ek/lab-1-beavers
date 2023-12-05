using System.ComponentModel.DataAnnotations;

namespace BaverGame.Domain.Entities;

public sealed class GameTag
{
    [Key]
    public Guid GameTagId { get; } = Guid.NewGuid();
    public Guid GameId { get; set; }
    public Guid TagId { get; set; }
    public Game Game { get; set; }
    public Tag Tag { get; set; }
}