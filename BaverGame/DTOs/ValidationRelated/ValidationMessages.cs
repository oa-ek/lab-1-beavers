namespace BaverGame.DTOs.ValidationRelated;

public static class ValidationMessages
{
    public const string RequiredField = "{0} is required.";
    public const string InvalidGuidFormat = "{0} must be a 36-character GUID.";
    public const string InvalidUrlFormat = "Invalid URL format.";
    public const string InvalidEmailFormat = "{0} has invalid email format.";
    public const string InvalidNumericValue = "Invalid {0} numeric value.";
    public const string InvalidValue = "Invalid {0} value.";
}