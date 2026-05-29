namespace RachaStats.Application.Matches.Responses;

public class MatchDetailResponse
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime EditableUntil { get; set; }
    public bool IsEditable { get; set; }
    public List<PlayerResponse> Players { get; set; } = new();
}
