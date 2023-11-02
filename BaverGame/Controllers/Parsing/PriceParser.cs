using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using BaverGame.Controllers.Parsing.Core.API;
using BaverGame.Controllers.Parsing.Core.Exceptions;

namespace BaverGame.Controllers.Parsing;

public sealed class PriceParser : IParser<decimal>
{
    private const string MoneyChar = "\u20b4";

    public decimal Parse(IHtmlDocument document, string[] priceElements)
    {
        string finalText = string.Empty;
        foreach(string priceElementKey in priceElements)
        {
            IElement? priceElement = document.QuerySelector(priceElementKey);
            string? text = priceElement?.TextContent ?? priceElement?.Text();
            if(text is null)
                continue;

            finalText = text;
            break;
        }

        if(decimal.TryParse(finalText, out decimal result))
            return result;

        finalText = finalText.Replace(MoneyChar, "");
        
        if(decimal.TryParse(finalText, out result))
            return result;
        
        throw new PriceElementNoFoundException();
    }
}