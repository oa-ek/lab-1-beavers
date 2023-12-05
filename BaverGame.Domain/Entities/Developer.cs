using System.ComponentModel.DataAnnotations;

namespace BaverGame.Domain.Entities;

public sealed class Developer
{
    [Key] 
    public Guid DeveloperId { get; } = Guid.NewGuid();
    [MaxLength(255)]
    public string DeveloperName { get; set; }
    public ICollection<Game> Games { get; set; }
}