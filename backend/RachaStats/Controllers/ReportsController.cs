using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RachaStats.Application.Reports;
using RachaStats.Application.Reports.Requests;

namespace RachaStats.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("frequency-ranking")]
    public async Task<IActionResult> GetFrequencyRanking([FromQuery] ReportFilterRequest filter)
    {
        var result = await _reportService.GetFrequencyRankingAsync(filter);
        return Ok(result);
    }
    
    [HttpGet("goals-ranking")]
    public async Task<IActionResult> GetGoalsRanking()
    {
        var result = await _reportService.GetGoalsRankingAsync();
        return Ok(result);
    }
}