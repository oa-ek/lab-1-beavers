namespace BaverGame.DTOs;

public sealed class ScreenshotDto
{
    public Guid ScreenshotId { get; } = Guid.NewGuid();
    public Guid GameId { get; set; }
    public string ImageUrl { get; set; }
}