namespace TaskManager.Application.Common;

public class ErrorResponse
{
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}