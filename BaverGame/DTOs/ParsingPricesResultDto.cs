using Core;

namespace BaverGame.DTOs;

public sealed class ParsingPricesResultDto
{
    public readonly int ParsedCount;
    public readonly List<Price> ErrorPrices;
    public readonly TimeSpan TimeSpent;
    public int ErrorsCount => ErrorPrices.Count;

    public ParsingPricesResultDto(int parsedCount, List<Price> errorPrices, TimeSpan timeSpent)
    {
        ParsedCount = parsedCount;
        ErrorPrices = errorPrices;
        TimeSpent = timeSpent;
    }
}