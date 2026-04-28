namespace TaskManager.Domain;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public bool IsComplete { get; set; }
}