using System.ComponentModel.DataAnnotations;
using BaverGame.DTOs.ValidationRelated;

namespace BaverGame.DTOs;

public sealed class PriceDto
{
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(36, MinimumLength = 36, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    [RegularExpression(RegexPatterns.GuidPattern, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    public string PriceId { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(36, MinimumLength = 36, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    [RegularExpression(RegexPatterns.GuidPattern, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    public string GameId { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(36, MinimumLength = 36, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    [RegularExpression(RegexPatterns.GuidPattern, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    public string StoreId { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [Range(0, double.MaxValue, ErrorMessage = ValidationMessages.InvalidNumericValue)]
    public decimal PriceValue { get; set; }

    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [RegularExpression(RegexPatterns.UrlPattern, ErrorMessage = ValidationMessages.InvalidUrlFormat)]
    public string PriceUrl { get; set; }

    public string CurrencyPostfix { get; set; }
}