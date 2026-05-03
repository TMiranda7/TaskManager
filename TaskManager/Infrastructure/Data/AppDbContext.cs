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
    public DbSet<Match> Matches { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<MatchStat> MatchStats { get; set; }
}