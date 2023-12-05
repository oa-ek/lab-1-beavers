using System.ComponentModel.DataAnnotations;
using BaverGame.Application.DTOs.ValidationRelated;

namespace BaverGame.Application.DTOs.AuthenticationRelated;

public class RegistrationFormDto : LoginFormDto
{
    [Required(ErrorMessage = ValidationMessages.RequiredField)]
    [RegularExpression(RegexPatterns.EmailPattern, ErrorMessage = ValidationMessages.InvalidEmailFormat)]
    public string Email { get; set; }
}