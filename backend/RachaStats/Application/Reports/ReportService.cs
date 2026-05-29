using Microsoft.EntityFrameworkCore;
using RachaStats.Application.Reports.Requests;
using RachaStats.Application.Reports.Responses;
using RachaStats.Infrastructure.Data;

namespace RachaStats.Application.Reports;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;
    
    public ReportService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<FrequencyRankingResponse>> GetFrequencyRankingAsync(ReportFilterRequest request)
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
}
