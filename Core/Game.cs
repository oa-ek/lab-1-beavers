using System.ComponentModel.DataAnnotations;

namespace Core;

public sealed class Game
{
    [Key]
    public Guid GameId { get; } = Guid.NewGuid();
    public Guid DeveloperId { get; set; }
    public Guid PublisherId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SystemRequirements { get; set; }
    public Developer Developer { get; set; }
    public Publisher Publisher { get; set; }
    public ICollection<GameTag> GameTags { get; set; }
    public ICollection<Price> Prices { get; set; }
    public ICollection<Screenshot> Screenshots { get; set; }
}