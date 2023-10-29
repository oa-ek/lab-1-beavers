using System.ComponentModel.DataAnnotations;
using BaverGame.DTOs.ValidationRelated;

namespace BaverGame.DTOs;

public sealed class UserDto
{
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(36, MinimumLength = 36, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    [RegularExpression(RegexPatterns.GuidPattern, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    public string UserId { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(36, MinimumLength = 36, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    [RegularExpression(RegexPatterns.GuidPattern, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    public string RoleId { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(255, MinimumLength = 3, ErrorMessage = ValidationMessages.InvalidValue)]
    public string Username { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [RegularExpression(RegexPatterns.EmailPattern, ErrorMessage = ValidationMessages.InvalidEmailFormat)]
    public string Email { get; set; }
    
    public string Password { get; set; } = "default";
}