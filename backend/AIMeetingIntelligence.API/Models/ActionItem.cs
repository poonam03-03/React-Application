namespace AIMeetingIntelligence.API.Models;

public class ActionItem
{
    public Guid Id { get; set; }

    public string Task { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public Guid MeetingId { get; set; }
        
    public Meeting? Meeting { get; set; }
}