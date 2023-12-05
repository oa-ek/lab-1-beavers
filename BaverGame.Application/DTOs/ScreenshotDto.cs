using System.ComponentModel.DataAnnotations;
using BaverGame.Application.DTOs.ValidationRelated;

namespace BaverGame.Application.DTOs;

public sealed class ScreenshotDto
{
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(36, MinimumLength = 36, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    [RegularExpression(RegexPatterns.GuidPattern, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    public string ScreenshotId { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(36, MinimumLength = 36, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    [RegularExpression(RegexPatterns.GuidPattern, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    public string GameId { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [RegularExpression(RegexPatterns.UrlPattern, ErrorMessage = ValidationMessages.InvalidUrlFormat)]
    public string ImageUrl { get; set; }
}