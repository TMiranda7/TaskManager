using TaskManager.Application.Matches.Requests;
using TaskManager.Application.Reports.Responses;

namespace TaskManager.Application.Reports;

public interface IReportService
{
    Task<List<FrequencyRankingResponse>> GetFrequencyRankingAsync();
    Task<List<GoalsRankingResponse>> GetGoalsRankingAsync();
}