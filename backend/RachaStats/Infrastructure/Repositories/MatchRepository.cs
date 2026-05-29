using Microsoft.EntityFrameworkCore;
using RachaStats.Application.Matches;
using RachaStats.Domain.Entities;
using RachaStats.Domain.Repositories;
using RachaStats.Infrastructure.Data;

namespace RachaStats.Infrastructure.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly AppDbContext _context;

    public MatchRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<MatchStat?> GetMatchStatAsync(Guid matchEntityId, Guid playerId)
    {
        return await _context.MatchStats.FirstOrDefaultAsync(x => x.MatchId == matchEntityId && x.PlayerId == playerId);
    }
    
    public async Task AddMatchAsync(Match match)
    {
        await _context.Matches.AddAsync(match);
    }

    public async Task<Match?> GetByIdAsync(Guid id)
    {
        return await _context.Matches.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Match?> GetByDateAsync(DateTime date)
    {
        var start = date.Date;
        var end = start.AddDays(1);

        return await _context.Matches.FirstOrDefaultAsync(x => x.Date >= start && x.Date < end);
    }

    public async Task<Match?> GetByMatchIdAsync(int matchId)
    {
        var date = MatchIdHelper.GetDateFromJulianMatchId(matchId);
        return await GetByDateAsync(date);
    }

    public async Task AddMatchStatAsync(MatchStat matchStat)
    {
        await _context.MatchStats.AddAsync(matchStat);
    }

    public async Task<Player?> GetPlayerByNameAsync(string name)
    {
        var normalizedName = PlayerNameNormalizer.Normalize(name);
        var players = await _context.Players.ToListAsync();

        return players.FirstOrDefault(x => PlayerNameNormalizer.Normalize(x.Name) == normalizedName);
    }

    public async Task<Player?> GetPlayerByIdAsync(Guid Id)
    {
        return await _context.Players.FirstOrDefaultAsync(x => x.Id == Id);
    }
    
    public async Task AddPlayerAsync(Player player)
    {
        await _context.Players.AddAsync(player);      
    }

    public async Task<Attendance?> GetAttendanceAsync(Guid matchEntityId, Guid playerId)
    {
        return await _context.Attendances.FirstOrDefaultAsync(x => x.MatchId == matchEntityId && x.PlayerId == playerId);
    }
    
    public async Task AddAttendanceAsync(Attendance attendance)
    {
        await _context.Attendances.AddAsync(attendance);
    }
    
    public async Task<Match?> GetByIdWithAttendancesAsync(Guid id)
    {
        return await _context.Matches
            .Include(m => m.Attendances)
            .ThenInclude(a => a.Player)
            .Include(m => m.Attendances)
            .ThenInclude(a => a.InvitedByPlayer)
            .Include(m => m.MatchStats)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Match?> GetByMatchIdWithAttendancesAsync(int matchId)
    {
        var date = MatchIdHelper.GetDateFromJulianMatchId(matchId);
        var start = date.Date;
        var end = start.AddDays(1);

        return await _context.Matches
            .Include(m => m.Attendances)
            .ThenInclude(a => a.Player)
            .Include(m => m.Attendances)
            .ThenInclude(a => a.InvitedByPlayer)
            .Include(m => m.MatchStats)
            .FirstOrDefaultAsync(m => m.Date >= start && m.Date < end);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();   
    }
}
