namespace TaskManager.Domain.Entities;

public class Attendance
{
    public Guid Id { get; set; }

    public Guid MatchId { get; set; }
    public Match Match { get; set; } = null!;

    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    
    public Guid? InvitedByPlayerId { get; set; }
    public Player? InvitedByPlayer { get; set; }
    public bool IsGoalkeeper { get; set; }

}