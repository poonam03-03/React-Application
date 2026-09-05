using AIMeetingIntelligence.API.Data;
using AIMeetingIntelligence.API.DTOs;
using AIMeetingIntelligence.API.Models;
using AIMeetingIntelligence.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIMeetingIntelligence.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeetingController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PdfService _pdf;


    public MeetingController(
    AppDbContext context,
    IServiceScopeFactory scopeFactory,
    PdfService pdf)
    {
        _context = context;
        _scopeFactory = scopeFactory;
        _pdf = pdf;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] MeetingUploadDto dto)
    {
        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        if (!Directory.Exists(uploads))
            Directory.CreateDirectory(uploads);

        var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
        var filePath = Path.Combine(uploads, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        var meeting = new Meeting
        {
            Title = dto.Title,
            FileName = dto.File.FileName,
            FilePath = fileName,
            Status = "Uploaded"
        };

        _context.Meetings.Add(meeting);
        await _context.SaveChangesAsync();
       
        _ = Task.Run(async () =>
        {
            using var scope = _scopeFactory.CreateScope();

            var processor = scope.ServiceProvider
                .GetRequiredService<MeetingProcessingService>();

            await processor.ProcessMeeting(meeting.Id);
        });

        return Ok(meeting);
    }

    [HttpGet]
    public async Task<IActionResult> GetMeetings()
    {
        return Ok(await _context.Meetings
            .OrderByDescending(x => x.UploadedAt)
            .ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var meeting = await _context.Meetings
            .Include(x => x.ActionItems)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (meeting == null)
            return NotFound();

        return Ok(new
        {
            meeting.Id,
            meeting.Title,
            meeting.Summary,

            ActionItems = meeting.ActionItems.Select(x => new
            {
                x.Id,
                x.Task,
                x.IsCompleted
            })
        });
    }

    [HttpPut("actionitem/{id}")]
    public async Task<IActionResult> ToggleActionItem(Guid id)
    {
        var item = await _context.ActionItems.FindAsync(id);

        if (item == null)
            return NotFound();

        item.IsCompleted = !item.IsCompleted;

        await _context.SaveChangesAsync();

        return Ok(item);
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> Download(Guid id)
    {
        var meeting = await _context.Meetings
            .Include(x => x.ActionItems)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (meeting == null)
            return NotFound();

        var bytes = _pdf.Generate(
            meeting.Title,
            meeting.Summary ?? "",
            meeting.ActionItems.Select(x => x.Task).ToList());

        return File(
            bytes,
            "application/pdf",
            $"{meeting.Title}.pdf");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var meeting = await _context.Meetings
            .Include(x => x.ActionItems)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (meeting == null)
            return NotFound();

        _context.ActionItems.RemoveRange(meeting.ActionItems);
        _context.Meetings.Remove(meeting);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}