using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core;

public class Price
{
    [Key]
    public Guid PriceId { get; } = Guid.NewGuid();
    public Guid GameId { get; set; }
    public Guid StoreId { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal PriceAmount { get; set; }
    [StringLength(255)]
    public string PriceUrl { get; set; }
    public Game Game { get; set; }
    public GameStore Store { get; set; }
}