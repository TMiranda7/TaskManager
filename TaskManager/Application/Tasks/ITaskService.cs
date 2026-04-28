using TaskManager.Application.Common.Pagination;
using TaskManager.Application.Tasks.Requests;
using TaskManager.Application.Tasks.Responses;

namespace TaskManager.Application.Tasks;

public interface ITaskService
{
    Task<PaginatedResponse<TaskResponse>> GetAllAsync(PaginationRequest request);
    Task<TaskResponse?> GetByIdAsync(Guid id);
    Task<TaskResponse> CreateAsync(CreateTaskRequest item);
    Task<TaskResponse?> UpdateAsync(Guid id, UpdateTaskRequest item);
    Task DeleteAsync(Guid id);
}