namespace AIMeetingIntelligence.API.DTOs;

public class MeetingUploadDto
{
    public string Title { get; set; } = string.Empty;

    public IFormFile File { get; set; } = null!;
}