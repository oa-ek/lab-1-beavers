using System.ComponentModel.DataAnnotations;

namespace BaverGame.Domain.Entities;

public sealed class Tag
{
    [Key]
    public Guid TagId { get; } = Guid.NewGuid();
    [MaxLength(255)]
    public string TagName { get; set; }
}