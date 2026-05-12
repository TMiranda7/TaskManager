using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories;

public interface IMatchRepository
{
    Task AddMatchAsync(Match match);
    Task<Match?> GetByIdAsync(Guid id);
    Task<Match?> GetByIdWithAttendancesAsync(Guid id);
    Task<Player?> GetPlayerByNameAsync(string name);
    Task<Player?> GetPlayerByIdAsync(Guid Id);
    Task<Attendance?> GetAttendanceAsync(Guid matchId,Guid playerId);
    Task<MatchStat?> GetMatchStatAsync(Guid matchId, Guid playerId);
    Task AddMatchStatAsync(MatchStat matchStat);
    Task AddPlayerAsync(Player player);
    Task AddAttendanceAsync(Attendance attendance);

    Task SaveChangesAsync();
}