using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Matches;
using TaskManager.Application.Matches.Requests;

namespace TaskManager.Controllers;

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
    public IActionResult GetById(Guid id)
    {
        var result = _importMatchService.GetByIdAsync(id);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpPost("{matchId}/goals")]
    public async Task<IActionResult> RegisterGoals(Guid matchId, [FromBody] RegisterGoalRequest request)
    {
        await _importMatchService.RegisterGoalsAsync(matchId, request);
        return NoContent();
    }
}