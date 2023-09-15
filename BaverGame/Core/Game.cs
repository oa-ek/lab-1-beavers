using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core;

public class Game
{
    [Key] 
    public Guid GameId { get; } = Guid.NewGuid();
    public Guid DeveloperId { get; set; }
    public Guid PublisherId { get; set; }
    public string Description { get; set; }
    public string SystemRequirements { get; set; }
    public Developer Developer { get; set; }
    public Publisher Publisher { get; set; }
    public ICollection<GameTag> GameTags { get; set; }
    public ICollection<UserGameOwnership> UserGameOwnerships { get; set; }
    public ICollection<Price> Prices { get; set; }
}