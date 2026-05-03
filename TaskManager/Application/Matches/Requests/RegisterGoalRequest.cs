namespace TaskManager.Application.Matches.Requests;

public class RegisterGoalRequest
{
    public Guid PlayerId { get; set; }
    public int Goals { get; set; }
}