using System.ComponentModel.DataAnnotations;

namespace Core;

public sealed class Screenshot
{
    [Key]
    public Guid ScreenshotId { get; } = Guid.NewGuid();
    public Guid GameId { get; set; }
    [MaxLength(255)]
    public string ImageUrl { get; set; }
    public Game Game { get; set; }
}