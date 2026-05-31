using Microsoft.EntityFrameworkCore;
using RachaStats.Application.Highlights.Responses;
using RachaStats.Domain.Entities;
using RachaStats.Infrastructure.Data;

namespace RachaStats.Application.Highlights;

public class HighlightService : IHighlightService
{
    private readonly AppDbContext _context;

    public HighlightService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<HighlightSuggestionResponse>> GetSuggestionsByMatchAsync(Guid matchId)
    {
        return await _context.HighlightSuggestions
            .Where(x => x.MatchId == matchId)
            .OrderByDescending(x => x.Confidence)
            .Select(x => new HighlightSuggestionResponse
            {
                Id = x.Id,
                MatchId = x.MatchId,
                PlayerId = x.PlayerId,
                SourceMessage = x.SourceMessage,
                Reason = x.Reason,
                Confidence = x.Confidence,
                Status = x.Status.ToString(),
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<Guid> CreateSuggestionAsync(
        Guid matchId,
        Guid playerId,
        string sourceMessage,
        string reason,
        decimal confidence)
    {
        var matchExists = await _context.Matches.AnyAsync(x => x.Id == matchId);

        if (!matchExists)
            throw new Exception("Racha não encontrado.");

        var playerExists = await _context.Players.AnyAsync(x => x.Id == playerId);

        if (!playerExists)
            throw new Exception("Jogador não encontrado.");

        var suggestion = new HighlightSuggestion
        {
            Id = Guid.NewGuid(),
            MatchId = matchId,
            PlayerId = playerId,
            SourceMessage = sourceMessage,
            Reason = reason,
            Confidence = confidence,
            Status = HighlightSuggestionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.HighlightSuggestions.Add(suggestion);

        await _context.SaveChangesAsync();

        return suggestion.Id;
    }

    public async Task ApproveAsync(Guid suggestionId)
    {
        var suggestion = await _context.HighlightSuggestions
            .FirstOrDefaultAsync(x => x.Id == suggestionId);

        if (suggestion == null)
            throw new Exception("Sugestão não encontrada.");

        suggestion.Status = HighlightSuggestionStatus.Approved;

        var stat = await _context.MatchStats
            .FirstOrDefaultAsync(x =>
                x.MatchId == suggestion.MatchId &&
                x.PlayerId == suggestion.PlayerId);

        if (stat == null)
        {
            stat = new MatchStat
            {
                Id = Guid.NewGuid(),
                MatchId = suggestion.MatchId,
                PlayerId = suggestion.PlayerId,
                Goals = 0,
                IsHighlight = true
            };

            _context.MatchStats.Add(stat);
        }
        else
        {
            stat.IsHighlight = true;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RejectAsync(Guid suggestionId)
    {
        var suggestion = await _context.HighlightSuggestions
            .FirstOrDefaultAsync(x => x.Id == suggestionId);

        if (suggestion == null)
            throw new Exception("Sugestão não encontrada.");

        suggestion.Status = HighlightSuggestionStatus.Rejected;

        await _context.SaveChangesAsync();
    }
}