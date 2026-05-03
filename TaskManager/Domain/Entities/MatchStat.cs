namespace TaskManager.Domain.Entities;

public class MatchStat
{
    public Guid Id { get; set; }

    public Guid MatchId { get; set; }
    public Match Match { get; set; } = null!;

    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public int Goals { get; set; }
    public bool IsHighlight { get; set; }
}