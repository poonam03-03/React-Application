using AIMeetingIntelligence.API.DTOs;
using AIMeetingIntelligence.API.Models;
using AIMeetingIntelligence.API.Repositories;
using AIMeetingIntelligence.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIMeetingIntelligence.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeetingController(
    IMeetingRepository meetingRepository,
    IServiceScopeFactory scopeFactory,
    PdfService pdf) : ControllerBase
{
    private readonly IMeetingRepository _meetingRepository = meetingRepository;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly PdfService _pdf = pdf;

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] MeetingUploadDto dto)
    {
        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        if (!Directory.Exists(uploads))
            Directory.CreateDirectory(uploads);

        var meeting = await _meetingRepository.UploadMeetingAsync(dto, uploads);

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
        var meetings = await _meetingRepository.GetMeetingsAsync();
        return Ok(meetings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var meeting = await _meetingRepository.GetMeetingDetailsAsync(id);

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
        var item = await _meetingRepository.ToggleActionItemAsync(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> Download(Guid id)
    {
        var meeting = await _meetingRepository.GetMeetingWithActionItemsAsync(id);

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
        var meeting = await _meetingRepository.GetMeetingWithActionItemsAsync(id);

        if (meeting == null)
            return NotFound();

        await _meetingRepository.DeleteMeetingAsync(meeting);

        return NoContent();
    }
}
