using System.ComponentModel.DataAnnotations;

namespace BaverGame.Domain.Entities;

public sealed class Price
{
    [Key]
    public Guid PriceId { get; } = Guid.NewGuid();
    public Guid GameId { get; set; }
    public Guid StoreId { get; set; }
    public decimal PriceValue { get; set; }

    public string CurrencyPostfix { get; set; }

    [MaxLength(255)]
    public string PriceUrl { get; set; }
    public Game Game { get; set; }
    public Store Store { get; set; }
}