using Microsoft.EntityFrameworkCore;
using TaskManager.Domain;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
            _context = context;
    }
    
    public async Task<List<TaskItem>> GetAllAsync(int pageNumber , int pageSize, string? title = null)
    {
        var query = _context.Tasks.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Title.Contains(title));
        
        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)           
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(TaskItem item)
    {
        await _context.Tasks.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (item != null)
        {
            _context.Tasks.Remove(item);
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public async Task<TaskItem?> UpdateAsync( TaskItem item)
    {
        var itemRes = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == item.Id);
        if (itemRes != null)
        {
            itemRes.Title = item.Title;
            itemRes.IsComplete = item.IsComplete;
            
            await _context.SaveChangesAsync(); 
            return itemRes;
        }
        return null;
    }

    public async Task<int> CountAsync(string? title = null)
    {
        var query = _context.Tasks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Title.Contains(title));
        
        return await query.CountAsync();
    }
}