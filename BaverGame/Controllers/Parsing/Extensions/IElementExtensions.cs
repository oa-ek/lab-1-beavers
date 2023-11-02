using AngleSharp.Dom;

namespace BaverGame.Controllers.Parsing.Extensions;

// ReSharper disable once InconsistentNaming
public static class IElementExtensions
{
    public static string? GetHref(this IElement element) =>
        element.GetAttribute("href");
}