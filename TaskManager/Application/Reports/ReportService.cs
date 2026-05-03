using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Matches.Requests;
using TaskManager.Application.Reports.Responses;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Application.Reports;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;
    
    public ReportService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<FrequencyRankingResponse>> GetFrequencyRankingAsync()
    {
        return await _context.Attendances
            .GroupBy(a => new
            {
                a.PlayerId,
                a.Player.Name
            })
            .Select(g => new FrequencyRankingResponse
            {
                PlayerId = g.Key.PlayerId,
                PlayerName = g.Key.Name,
                AttendanceCount = g.Count()
            })
            .OrderByDescending(x => x.AttendanceCount)
            .ThenBy(x => x.PlayerName)
            .ToListAsync();
    }


    public async Task<List<GoalsRankingResponse>> GetGoalsRankingAsync()
    {
        return await _context.MatchStats
            .GroupBy(x => new
            {
                x.PlayerId,
                x.Player.Name
            })
            .Select(g => new GoalsRankingResponse
            {
                PlayerId = g.Key.PlayerId,
                PlayerName = g.Key.Name,
                GoalsCount = g.Sum(x => x.Goals)
            })
            .OrderByDescending(x => x.GoalsCount)
            .ThenBy(x => x.PlayerName)
            .ToListAsync();
    }

    public async Task RegisterGoalsAsync(Guid matchId, RegisterGoalRequest request)
    {
        if (request.Goals < 0)
            throw new BusinessException("A quantidade de gols não pode ser negativa.");

        var match = await _context.Matches
            .FirstOrDefaultAsync(x => x.Id == matchId);

        if (match == null)
            throw new NotFoundException("Racha não encontrado.");

        var player = await _context.Players
            .FirstOrDefaultAsync(x => x.Id == request.PlayerId);

        if (player == null)
            throw new NotFoundException("Jogador não encontrado.");

        var attendance = await _context.Attendances
            .FirstOrDefaultAsync(x => x.MatchId == matchId && x.PlayerId == request.PlayerId);

        if (attendance == null)
            throw new BusinessException("Esse jogador não está presente nesse racha.");

        var stat = await _context.MatchStats
            .FirstOrDefaultAsync(x => x.MatchId == matchId && x.PlayerId == request.PlayerId);

        if (stat == null)
        {
            stat = new MatchStat
            {
                Id = Guid.NewGuid(),
                MatchId = matchId,
                PlayerId = request.PlayerId,
                Goals = request.Goals,
                IsHighlight = false
            };

            _context.MatchStats.Add(stat);
        }
        else
        {
            stat.Goals = request.Goals;
        }

        await _context.SaveChangesAsync();
    }
}