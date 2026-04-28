using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Pagination;
using TaskManager.Application.Tasks.Requests;
using TaskManager.Application.Tasks.Responses;
using TaskManager.Domain;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Tasks;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedResponse<TaskResponse>> GetAllAsync(PaginationRequest request)
    {
        if (request.PageNumber <= 0)
            request.PageNumber = 1;
        
        if (request.PageSize <= 0)
            request.PageSize = 10;
        
        var itens = await _repository.GetAllAsync(request.PageNumber, 
            request.PageSize,
            request.Title);
        
        var totalItens = await _repository.CountAsync(request.Title);

        return new PaginatedResponse<TaskResponse>
        {
            Itens = itens.Select(MapToTaskReponse).ToList(),
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalItens = totalItens,
            TotalCount = totalItens
        };
    }

    public async Task<TaskResponse?> GetByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item == null)
            throw new NotFoundException("Tarefa nao encontrada");
        
        return MapToTaskReponse(item);
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskRequest item)
    {
        if (string.IsNullOrWhiteSpace(item.Title))
            throw new BusinessException("Titulo é obrigatorio");
        
        var task = new TaskItem();
        
        if (!string.IsNullOrWhiteSpace(item.Title))
        {
            task.Id = Guid.NewGuid();
            task.Title = item.Title;
        }

        await _repository.AddAsync(task);
        return MapToTaskReponse(task);
    }

    public async Task DeleteAsync(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);
        
        if (!deleted)
            throw new BusinessException("Tarefa nao encontrada");
    }

    public async Task<TaskResponse?> UpdateAsync(Guid id, UpdateTaskRequest item)
    {
        if (string.IsNullOrWhiteSpace(item.Title))
            throw new BusinessException("Titulo é obrigatorio");
        
        var itemExiste = await _repository.GetByIdAsync(id);

        if (itemExiste == null) 
            throw new NotFoundException("Tarefa nao encontrada");
        
        itemExiste.Title = item.Title;
        itemExiste.IsComplete = item.IsCompleted;

        var atualizado = await _repository.UpdateAsync(itemExiste);
        
        return MapToTaskReponse(atualizado);
    }

    private static TaskResponse MapToTaskReponse(TaskItem item)
    {
        return new TaskResponse
        {
            Id = item.Id,
            Title = item.Title,
            IsComplete = item.IsComplete,
        };
    } 
}