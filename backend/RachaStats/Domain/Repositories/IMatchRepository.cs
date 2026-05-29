using RachaStats.Domain.Entities;

namespace RachaStats.Domain.Repositories;

public interface IMatchRepository
{
    Task AddMatchAsync(Match match);
    Task<Match?> GetByIdAsync(Guid id);
    Task<Match?> GetByDateAsync(DateTime date);
    Task<Match?> GetByMatchIdAsync(int matchId);
    Task<Match?> GetByIdWithAttendancesAsync(Guid id);
    Task<Match?> GetByMatchIdWithAttendancesAsync(int matchId);
    Task<Player?> GetPlayerByNameAsync(string name);
    Task<Player?> GetPlayerByIdAsync(Guid Id);
    Task<Attendance?> GetAttendanceAsync(Guid matchEntityId,Guid playerId);
    Task<MatchStat?> GetMatchStatAsync(Guid matchEntityId, Guid playerId);
    Task AddMatchStatAsync(MatchStat matchStat);
    Task AddPlayerAsync(Player player);
    Task AddAttendanceAsync(Attendance attendance);

    Task SaveChangesAsync();
}
