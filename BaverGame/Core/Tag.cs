using System.ComponentModel.DataAnnotations;

namespace Core;

public class Tag
{
    [Key]
    public Guid TagId { get; } = Guid.NewGuid();
    [StringLength(255)]
    public string TagName { get; set; }
    public ICollection<GameTag> GameTags { get; set; }
}