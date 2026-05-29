namespace RachaStats.Application.Matches.Responses;

public class ImportedPlayer
{
    public string Name { get; set; } = string.Empty;
    public string? InvitedBy { get; set; }
    public bool IsGoalkeeper { get; set; }
}