using RachaStats.Application.Matches.Requests;
using RachaStats.Application.Matches.Responses;

namespace RachaStats.Application.Matches;

public interface IImportMatchService
{
    Task<MatchDetailResponse?> GetByIdAsync(int matchId);
    Task<int> ImportAsync(ImportMatchRequest request);
    Task RegisterGoalsAsync(int matchId, RegisterGoalRequest request);
}
