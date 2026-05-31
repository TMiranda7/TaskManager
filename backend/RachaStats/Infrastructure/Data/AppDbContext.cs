using Microsoft.EntityFrameworkCore;
using RachaStats.Domain.Entities;

namespace RachaStats.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    { }
    
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<MatchStat> MatchStats { get; set; }
    
    public DbSet<HighlightSuggestion> HighlightSuggestions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("AppUsers");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Username).HasMaxLength(100).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(64).IsRequired();
            entity.Property(user => user.Role).HasMaxLength(20).IsRequired();
            entity.HasIndex(user => user.Username).IsUnique();
        });
    }
}
