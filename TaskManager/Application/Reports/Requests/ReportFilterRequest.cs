namespace TaskManager.Application.Reports.Requests;

public class ReportFilterRequest
{
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
}