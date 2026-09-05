using AIMeetingIntelligence.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIMeetingIntelligence.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(IDashboardRepository dashboardRepository) : ControllerBase
{
    private readonly IDashboardRepository _dashboardRepository = dashboardRepository;

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var total = await _dashboardRepository.GetTotalMeetingsAsync();
        var completed = await _dashboardRepository.GetCompletedMeetingsAsync();
        var processing = await _dashboardRepository.GetProcessingMeetingsAsync();
        var uploaded = await _dashboardRepository.GetUploadedMeetingsAsync();

        return Ok(new
        {
            total,
            completed,
            processing,
            uploaded
        });
    }
}

