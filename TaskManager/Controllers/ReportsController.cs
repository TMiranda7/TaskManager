using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Reports;

namespace TaskManager.Controllers;

[Authorize]
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
    public async Task<IActionResult> GetFrequencyRanking()
    {
        var result = await _reportService.GetFrequencyRankingAsync();
        return Ok(result);
    }
    
    [HttpGet("goals-ranking")]
    public async Task<IActionResult> GetGoalsRanking()
    {
        var result = await _reportService.GetGoalsRankingAsync();
        return Ok(result);
    }
}