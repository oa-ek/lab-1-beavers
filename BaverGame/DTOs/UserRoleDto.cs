using System.ComponentModel.DataAnnotations;
using BaverGame.DTOs.ValidationRelated;

namespace BaverGame.DTOs;

public sealed class UserRoleDto
{
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(36, MinimumLength = 36, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    [RegularExpression(RegexPatterns.GuidPattern, ErrorMessage = ValidationMessages.InvalidGuidFormat)]
    public string RoleId { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(255, MinimumLength = 3, ErrorMessage = ValidationMessages.InvalidValue)]
    public string RoleName { get; set; }
}