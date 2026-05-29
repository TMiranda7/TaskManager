namespace RachaStats.Application.Reports.Responses;

public class GoalsRankingResponse
{
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int GoalsCount { get; set; }
}