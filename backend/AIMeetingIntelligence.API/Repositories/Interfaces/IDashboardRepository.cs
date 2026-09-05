namespace AIMeetingIntelligence.API.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalMeetingsAsync();
        Task<int> GetCompletedMeetingsAsync();
        Task<int> GetProcessingMeetingsAsync();
        Task<int> GetUploadedMeetingsAsync();
    }
}



