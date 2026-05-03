namespace TaskManager.Application.Reports.Responses;

public class FrequencyRankingResponse
{
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int AttendanceCount { get; set; }
}