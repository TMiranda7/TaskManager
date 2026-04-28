using Microsoft.EntityFrameworkCore;
using TaskManager.Domain;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    { }
    
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}