using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace BaverGame.Controllers.Parsing.Extensions;

public static class HtmlDocumentExtensions
{
    public static IElement? FindClass(this IHtmlDocument document, string className) =>
        document.QuerySelector("." + className);
    
    public static IHtmlCollection<IElement> FindClasses(this IHtmlDocument document, string className) =>
        document.QuerySelectorAll("." + className);
}