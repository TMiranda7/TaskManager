using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.Pagination;
using TaskManager.Application.Tasks;
using TaskManager.Application.Tasks.Requests;


namespace TaskManager.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _service;

    public TaskController(ITaskService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]PaginationRequest request)
    {
        var res = await _service.GetAllAsync(request);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var res = await _service.GetByIdAsync(id);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest item)
    {
            var createTask = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = createTask.Id }, createTask);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,[FromBody]UpdateTaskRequest item)
    {
           var updateTask = await _service.UpdateAsync(id, item);
           return Ok(updateTask);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
         await _service.DeleteAsync(id);
         return NoContent();
    }
}