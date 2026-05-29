namespace RachaStats.Domain.Entities;

public class Match
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public string RawText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<Attendance> Attendances { get; set; } = new();
    public List<MatchStat> MatchStats { get; set; } = new();
}