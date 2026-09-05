using AIMeetingIntelligence.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIMeetingIntelligence.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnalyticsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAnalytics()
    {
        var meetings = await _context.Meetings.ToListAsync();

        var trend = meetings
            .GroupBy(x => x.UploadedAt.Date)
            .Select(g => new
            {
                date = g.Key.ToString("dd MMM"),
                count = g.Count()
            })
            .OrderBy(x => x.date);

        return Ok(new
        {
            total = meetings.Count,
            completed = meetings.Count(x => x.Status == "Completed"),
            processing = meetings.Count(x => x.Status == "Processing"),
            uploaded = meetings.Count(x => x.Status == "Uploaded"),
            trend
        });
    }
}