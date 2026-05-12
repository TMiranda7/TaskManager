using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly AppDbContext _context;

    public MatchRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<MatchStat?> GetMatchStatAsync(Guid matchId, Guid playerId)
    {
        return await _context.MatchStats.FirstOrDefaultAsync(x => x.Id == matchId && x.PlayerId == playerId);
    }
    
    public async Task AddMatchAsync(Match match)
    {
        await _context.Matches.AddAsync(match);
    }

    public async Task<Match?> GetByIdAsync(Guid id)
    {
        return await _context.Matches.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddMatchStatAsync(MatchStat matchStat)
    {
        await _context.MatchStats.AddAsync(matchStat);
    }

    public async Task<Player?> GetPlayerByNameAsync(string name)
    {
        return _context.Players.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
    }

    public async Task<Player?> GetPlayerByIdAsync(Guid Id)
    {
        return await _context.Players.FirstOrDefaultAsync(x => x.Id == Id);
    }
    
    public async Task AddPlayerAsync(Player player)
    {
        await _context.Players.AddAsync(player);      
    }

    public async Task<Attendance?> GetAttendanceAsync(Guid matchId, Guid playerId)
    {
        return await _context.Attendances.FirstOrDefaultAsync(x => x.Id == matchId && x.PlayerId == playerId);
    }
    
    public async Task AddAttendanceAsync(Attendance attendance)
    {
        _context.Attendances.AddAsync(attendance);
    }
    
    public async Task<Match?> GetByIdWithAttendancesAsync(Guid id)
    {
        return await _context.Matches
            .Include(m => m.Attendances)
            .ThenInclude(a => a.Player)
            .Include(m => m.Attendances)
            .ThenInclude(a => a.InvitedByPlayer)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();   
    }
}