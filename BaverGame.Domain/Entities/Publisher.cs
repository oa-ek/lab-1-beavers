using System.ComponentModel.DataAnnotations;

namespace BaverGame.Domain.Entities;

public sealed class Publisher
{
    [Key]
    public Guid PublisherId { get; } = Guid.NewGuid();
    [MaxLength(255)]
    public string PublisherName { get; set; }
    public ICollection<Game> Games { get; set; }
}