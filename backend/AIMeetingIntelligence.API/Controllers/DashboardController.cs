using AIMeetingIntelligence.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIMeetingIntelligence.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var total = await _context.Meetings.CountAsync();
        var completed = await _context.Meetings.CountAsync(x => x.Status == "Completed");
        var processing = await _context.Meetings.CountAsync(x => x.Status == "Processing");
        var uploaded = await _context.Meetings.CountAsync(x => x.Status == "Uploaded");

        return Ok(new
        {
            total,
            completed,
            processing,
            uploaded
        });
    }
}