namespace BaverGame.DTOs;

public sealed class ParsingPricesResultDto
{
    public readonly int ParsedCount;
    public readonly TimeSpan TimeSpent;

    public ParsingPricesResultDto(int parsedCount, TimeSpan timeSpent)
    {
        ParsedCount = parsedCount;
        TimeSpent = timeSpent;
    }
}