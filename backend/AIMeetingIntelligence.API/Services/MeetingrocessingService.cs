using AIMeetingIntelligence.API.Data;
using AIMeetingIntelligence.API.Hubs;
using AIMeetingIntelligence.API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AIMeetingIntelligence.API.Services;

public class MeetingProcessingService
{
    private readonly AppDbContext _context;
    private readonly IHubContext<NotificationHub> _hub;

    public MeetingProcessingService(
        AppDbContext context,
        IHubContext<NotificationHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    public async Task ProcessMeeting(Guid meetingId)
    {
        var meeting = await _context.Meetings.FindAsync(meetingId);

        if (meeting == null) return;

        // Step 1: Processing
        meeting.Status = "Processing";
        await _context.SaveChangesAsync();

        await _hub.Clients.All.SendAsync(
            "MeetingUpdated",
            meeting.Id,
            meeting.Status);

        await Task.Delay(3000);

        // Step 2: Read uploaded file
        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads",
            meeting.FilePath);

        string text = "";

        if (File.Exists(path))
            text = await File.ReadAllTextAsync(path);

        // Step 3: Generate AI data
        meeting.Summary = GenerateSummary(text);
        meeting.Status = "Completed";

        _context.ActionItems.AddRange(
            ExtractTasks(text, meeting.Id));

        await _context.SaveChangesAsync();

        // Step 4: Notify React
        await _hub.Clients.All.SendAsync(
            "MeetingUpdated",
            meeting.Id,
            meeting.Status);
    }
    private string GenerateSummary(string text)
    {
        var lines = text
            .Split('\n')
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Where(x => !x.StartsWith("TODO"))
            .Where(x => !x.StartsWith("Action"))
            .Where(x => !x.StartsWith("Task"))
            .Take(4);

        return string.Join(" ", lines);
    }

    private List<ActionItem> ExtractTasks(
        string text,
        Guid meetingId)
    {
        var tasks = new List<ActionItem>();

        var lines = text.Split('\n');

        foreach (var line in lines)
        {
            if (line.Contains("TODO") ||
                line.Contains("Action") ||
                line.Contains("Task"))
            {
                tasks.Add(new ActionItem
                {
                    MeetingId = meetingId,
                    Task = line.Trim(),
                    IsCompleted = false
                });
            }
        }

        if (!tasks.Any())
        {
            tasks.Add(new ActionItem
            {
                MeetingId = meetingId,
                Task = "Review meeting notes"
            });

            tasks.Add(new ActionItem
            {
                MeetingId = meetingId,
                Task = "Follow up with team"
            });
        }

        return tasks;
    }
}