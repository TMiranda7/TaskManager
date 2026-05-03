namespace TaskManager.Application.Matches.Responses;

public class MatchDetailResponse
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public List<PlayerResponse> Players { get; set; }
}