namespace RachaStats.Application.Highlights.Requests;

public class CreateHighlightSuggestionRequest
{
    public Guid MatchId { get; set; }

    public Guid PlayerId { get; set; }

    public string SourceMessage { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    public decimal Confidence { get; set; }
}