namespace TaskManager.Application.Tasks.Responses;

public class TaskResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsComplete { get; set; }
}