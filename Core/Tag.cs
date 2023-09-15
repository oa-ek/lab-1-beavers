using System.ComponentModel.DataAnnotations;

namespace Core;

public sealed class Tag
{
    [Key]
    public Guid TagId { get; } = Guid.NewGuid();
    [MaxLength(255)]
    public string TagName { get; set; }
    public ICollection<GameTag> GameTags { get; set; }
}