using RachaStats.Application.Highlights.Responses;

namespace RachaStats.Application.Highlights;

public interface IHighlightService
{
    Task<List<HighlightSuggestionResponse>> GetSuggestionsByMatchAsync(Guid matchId);
    Task<Guid> CreateSuggestionAsync(Guid matchId, Guid playerId, string sourceMessage, string reason, decimal confidence);
    Task ApproveAsync(Guid suggestionId);
    Task RejectAsync(Guid suggestionId);
}