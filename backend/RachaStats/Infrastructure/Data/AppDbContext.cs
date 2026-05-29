using Microsoft.EntityFrameworkCore;
using RachaStats.Domain.Entities;

namespace RachaStats.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    { }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<MatchStat> MatchStats { get; set; }
}
