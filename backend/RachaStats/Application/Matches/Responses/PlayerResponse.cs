namespace RachaStats.Application.Matches.Responses;

public class PlayerResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? InvitedBy { get; set; }
    public int Goals { get; set; }
}