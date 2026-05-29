using RachaStats.Application.Matches.Requests;
using RachaStats.Application.Reports.Requests;
using RachaStats.Application.Reports.Responses;

namespace RachaStats.Application.Reports;

public interface IReportService
{
    Task<List<FrequencyRankingResponse>> GetFrequencyRankingAsync(ReportFilterRequest request);
    Task<List<GoalsRankingResponse>> GetGoalsRankingAsync();
}