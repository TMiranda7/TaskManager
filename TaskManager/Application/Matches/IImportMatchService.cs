using TaskManager.Application.Matches.Requests;
using TaskManager.Application.Matches.Responses;

namespace TaskManager.Application.Matches;

public interface IImportMatchService
{
    Task<MatchDetailResponse?> GetByIdAsync(Guid id);
    Task<Guid> ImportAsync(ImportMatchRequest request);
    Task RegisterGoalsAsync(Guid matchId, RegisterGoalRequest request);
}