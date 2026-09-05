public class MeetingDetailsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public List<string> ActionItems { get; set; } = new();
}