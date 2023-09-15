using System.ComponentModel.DataAnnotations;

namespace Core;

public class GameStore
{
    [Key]
    public Guid StoreId { get; } = Guid.NewGuid();
    [StringLength(255)]
    public string StoreName { get; set; }
    public ICollection<Price> Prices { get; set; }
}