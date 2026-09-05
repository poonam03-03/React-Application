using AIMeetingIntelligence.API.DTOs;
using AIMeetingIntelligence.API.Models;
namespace AIMeetingIntelligence.API.Repositories;

public interface IMeetingRepository
{
    Task<Meeting> UploadMeetingAsync(MeetingUploadDto dto, string uploadsFolder);
    Task<List<Meeting>> GetMeetingsAsync();
    Task<Meeting?> GetMeetingDetailsAsync(Guid id);
    Task<ActionItem?> ToggleActionItemAsync(Guid id);
    Task<Meeting?> GetMeetingWithActionItemsAsync(Guid id);
    Task DeleteMeetingAsync(Meeting meeting);
}
