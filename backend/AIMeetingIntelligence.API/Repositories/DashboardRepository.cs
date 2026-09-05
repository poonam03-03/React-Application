using AIMeetingIntelligence.API.Data;
using AIMeetingIntelligence.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AIMeetingIntelligence.API.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _context;

    public DashboardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalMeetingsAsync()
    {
        return await _context.Meetings.CountAsync();
    }

    public async Task<int> GetCompletedMeetingsAsync()
    {
        return await _context.Meetings.CountAsync(x => x.Status == "Completed");
    }

    public async Task<int> GetProcessingMeetingsAsync()
    {
        return await _context.Meetings.CountAsync(x => x.Status == "Processing");
    }

    public async Task<int> GetUploadedMeetingsAsync()
    {
        return await _context.Meetings.CountAsync(x => x.Status == "Uploaded");
    }
}

