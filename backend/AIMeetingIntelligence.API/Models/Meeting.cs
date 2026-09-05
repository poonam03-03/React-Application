using System.ComponentModel.DataAnnotations;

namespace AIMeetingIntelligence.API.Models;

public class Meeting
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = "";

    public string FileName { get; set; } = "";

    public string FilePath { get; set; } = "";

    public string Status { get; set; } = "Uploaded";

    public string? Summary { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ActionItem> ActionItems { get; set; }
       = new List<ActionItem>();
}