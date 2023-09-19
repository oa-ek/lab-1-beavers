using System.ComponentModel.DataAnnotations;

namespace BaverGame.DTOs;

public sealed class PriceDto
{
    [StringLength(36, MinimumLength = 36, ErrorMessage = "PriceId must be a 36-character GUID.")]
    [RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", 
        ErrorMessage = "GameId is not in a valid GUID format.")]
    public string PriceId { get; set; }
    
    [Required(ErrorMessage = "GameId is required.")]
    [StringLength(36, MinimumLength = 36, ErrorMessage = "GameId must be a 36-character GUID.")]
    [RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", 
        ErrorMessage = "GameId is not in a valid GUID format.")]
    public string GameId { get; set; }

    [Required(ErrorMessage = "StoreId is required.")]
    [StringLength(36, MinimumLength = 36, ErrorMessage = "StoreId must be a 36-character GUID.")]
    [RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4]-[0-9a-fA-F]{4}-[0-9a-fA-F]{4]-[0-9a-fA-F]{12}$",
        ErrorMessage = "StoreId is not in a valid GUID format.")]
    public string StoreId { get; set; }

    [Required(ErrorMessage = "PriceValue is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "PriceValue must be greater than 0.")]
    public decimal PriceValue { get; set; }

    [Required(ErrorMessage = "PriceUrl is required.")]
    [RegularExpression(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$", 
        ErrorMessage = "Invalid URL format.")]
    public string PriceUrl { get; set; }
}