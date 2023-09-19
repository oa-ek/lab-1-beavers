namespace BaverGame.DTOs.ValidationRelated;

public static class RegexPatterns
{
    public const string GuidPattern = @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";
    public const string UrlPattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";
    public const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
}