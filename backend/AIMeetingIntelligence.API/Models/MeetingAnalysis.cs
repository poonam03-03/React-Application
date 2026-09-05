namespace AIMeetingIntelligence.API.Models;

public class MeetingAnalysis
{
    public int Id { get; set; }

    public int MeetingId { get; set; }

    public string Summary { get; set; } = string.Empty;

    public string? Sentiment { get; set; }

    public int Score { get; set; }

    public string? KeyTopics { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Meeting Meeting { get; set; } = null!;
}