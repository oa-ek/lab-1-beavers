using Core;

namespace BaverGame.DTOs;

public sealed class GameCatalogDto
{
    public IEnumerable<Game> Games { get; set; }
    public Guid? DeveloperId { get; set; } = Guid.Empty;
    public Guid? PublisherId { get; set; } = Guid.Empty;
    public Guid? TagId { get; set; } = Guid.Empty;
    public string OwnershipOption { get; set; }
}