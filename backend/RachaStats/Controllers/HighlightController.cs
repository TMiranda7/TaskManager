using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RachaStats.Application.Highlights;
using RachaStats.Application.Highlights.Requests;

namespace RachaStats.Controllers;

[Authorize]
[ApiController]
[Route("api/highlights")]
public class HighlightsController : ControllerBase
{
    private readonly IHighlightService _highlightService;

    public HighlightsController(IHighlightService highlightService)
    {
        _highlightService = highlightService;
    }

    [HttpGet("match/{matchId}")]
    public async Task<IActionResult> GetSuggestionsByMatch(Guid matchId)
    {
        var result = await _highlightService.GetSuggestionsByMatchAsync(matchId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSuggestion([FromBody] CreateHighlightSuggestionRequest request)
    {
        var suggestionId = await _highlightService.CreateSuggestionAsync(
            request.MatchId,
            request.PlayerId,
            request.SourceMessage,
            request.Reason,
            request.Confidence
        );

        return CreatedAtAction(
            nameof(GetSuggestionsByMatch),
            new { matchId = request.MatchId },
            new { id = suggestionId }
        );
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _highlightService.ApproveAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _highlightService.RejectAsync(id);
        return NoContent();
    }
}