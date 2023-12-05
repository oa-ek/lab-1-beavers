using System.ComponentModel.DataAnnotations;
using BaverGame.Application.DTOs.ValidationRelated;

namespace BaverGame.Application.DTOs.AuthenticationRelated;

public class LoginFormDto
{
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(255, MinimumLength = 3, ErrorMessage = ValidationMessages.InvalidValue)]
    public string InputUsername { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [StringLength(255, MinimumLength = 3, ErrorMessage = ValidationMessages.InvalidValue)]
    public string InputPassword { get; set; }
}