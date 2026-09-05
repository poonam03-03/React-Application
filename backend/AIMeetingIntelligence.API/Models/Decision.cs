namespace AIMeetingIntelligence.API.Models;

public class Decision
{
    public int Id { get; set; }

    public int MeetingId { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Meeting Meeting { get; set; } = null!;
}