using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core;

public sealed class Store
{
    [Key]
    public Guid StoreId { get; } = Guid.NewGuid();
    [MaxLength(255)]
    public string StoreName { get; set; }
    public ICollection<Price> Prices { get; set; }
}