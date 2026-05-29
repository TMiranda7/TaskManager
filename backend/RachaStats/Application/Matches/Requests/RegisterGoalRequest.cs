namespace RachaStats.Application.Matches.Requests;

public class RegisterGoalRequest
{
    public int MatchId { get; set; }
    public Guid PlayerId { get; set; }
    public int Goals { get; set; }
}
