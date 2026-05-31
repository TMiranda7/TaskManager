namespace RachaStats.Domain.Entities;

public enum HighlightSuggestionStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}

public class HighlightSuggestion
{
    public Guid Id { get; set; }

    public Guid MatchId { get; set; }
    public Match? Match { get; set; }

    public Guid PlayerId { get; set; }
    public Player? Player { get; set; }

    public string SourceMessage { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    public decimal Confidence { get; set; }

    public HighlightSuggestionStatus Status { get; set; } = HighlightSuggestionStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}