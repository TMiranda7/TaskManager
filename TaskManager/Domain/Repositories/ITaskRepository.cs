namespace TaskManager.Domain.Repositories;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync(int pageSize, int pageNumber, string? title = null);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task AddAsync(TaskItem item);
    Task<bool> DeleteAsync(Guid id);
    Task<TaskItem?> UpdateAsync(TaskItem item);
    Task<int> CountAsync(string? title = null);
}