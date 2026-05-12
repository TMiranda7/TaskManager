using TaskManager.Application.Matches.Requests;
using TaskManager.Application.Reports.Requests;
using TaskManager.Application.Reports.Responses;

namespace TaskManager.Application.Reports;

public interface IReportService
{
    Task<List<FrequencyRankingResponse>> GetFrequencyRankingAsync(ReportFilterRequest request);
    Task<List<GoalsRankingResponse>> GetGoalsRankingAsync();
}