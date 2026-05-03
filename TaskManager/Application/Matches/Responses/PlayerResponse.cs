namespace TaskManager.Application.Matches.Responses;

public class PlayerResponse
{
    public string Name { get; set; } = string.Empty;
    public string? InvitedBy { get; set; }
}