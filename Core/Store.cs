using System.ComponentModel.DataAnnotations;

namespace Core;

public sealed class Store
{
    [Key]
    public Guid StoreId { get; } = Guid.NewGuid();
    [MaxLength(255)]
    public string StoreName { get; set; }
    public ICollection<Price> Prices { get; set; }
    public string PriceElements { get; set; }
}