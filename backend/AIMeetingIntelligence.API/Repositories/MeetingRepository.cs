// AIMeetingIntelligence.API/Repositories/MeetingRepository.cs
using AIMeetingIntelligence.API.Data;
using AIMeetingIntelligence.API.DTOs;
using AIMeetingIntelligence.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AIMeetingIntelligence.API.Repositories;

public class MeetingRepository(AppDbContext context) : IMeetingRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Meeting> UploadMeetingAsync(MeetingUploadDto dto, string uploadsFolder)
    {
        var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);

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

        return meeting;
    }

    public async Task<List<Meeting>> GetMeetingsAsync()
    {
        return await _context.Meetings
            .OrderByDescending(x => x.UploadedAt)
            .ToListAsync();
    }

    public async Task<Meeting?> GetMeetingDetailsAsync(Guid id)
    {
        return await _context.Meetings
            .Include(x => x.ActionItems)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ActionItem?> ToggleActionItemAsync(Guid id)
    {
        var item = await _context.ActionItems.FindAsync(id);

        if (item == null)
            return null;

        item.IsCompleted = !item.IsCompleted;
        await _context.SaveChangesAsync();

        return item;
    }

    public async Task<Meeting?> GetMeetingWithActionItemsAsync(Guid id)
    {
        return await _context.Meetings
            .Include(x => x.ActionItems)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task DeleteMeetingAsync(Meeting meeting)
    {
        _context.ActionItems.RemoveRange(meeting.ActionItems);
        _context.Meetings.Remove(meeting);
        await _context.SaveChangesAsync();
    }
}



