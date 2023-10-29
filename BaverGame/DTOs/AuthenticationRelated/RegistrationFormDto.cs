using System.ComponentModel.DataAnnotations;
using BaverGame.DTOs.ValidationRelated;

namespace BaverGame.DTOs.AuthenticationRelated;

public class RegistrationFormDto : LoginFormDto
{
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [RegularExpression(RegexPatterns.EmailPattern, ErrorMessage = ValidationMessages.InvalidEmailFormat)]
    public string Email { get; set; }
}