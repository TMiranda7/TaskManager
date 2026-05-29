using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RachaStats.Application.Matches;
using RachaStats.Application.Matches.Requests;

namespace RachaStats.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IImportMatchService _importMatchService;

    public MatchesController(IImportMatchService importMatchService)
    {
        _importMatchService = importMatchService;
    }

    [HttpPost("import-whatsapp")]
    public async Task<IActionResult> ImportFromWhatsApp([FromBody] ImportMatchRequest request)
    {
        var matchId = await _importMatchService.ImportAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = matchId },
            new { id = matchId }
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _importMatchService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
    
    [HttpPost("{matchId}/goals")]
    public async Task<IActionResult> RegisterGoals(int matchId, [FromBody] RegisterGoalRequest request)
    {
        await _importMatchService.RegisterGoalsAsync(matchId, request);
        return NoContent();
    }
}
