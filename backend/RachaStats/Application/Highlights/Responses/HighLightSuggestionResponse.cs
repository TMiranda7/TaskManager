namespace RachaStats.Application.Highlights.Responses;

public class HighlightSuggestionResponse
{
    public Guid Id { get; set; }

    public Guid MatchId { get; set; }

    public Guid PlayerId { get; set; }

    public string SourceMessage { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    public decimal Confidence { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}