using TaskManager.Domain;

namespace TaskManager.Infrastructure.Storage;

public class TaskInMemoryStore
{
    public List<TaskItem> List { get; set; } = new();
}