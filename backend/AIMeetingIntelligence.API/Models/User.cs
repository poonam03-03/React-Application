using System.ComponentModel.DataAnnotations;

namespace AIMeetingIntelligence.API.Models;

public class User
{
    public Guid Id { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}