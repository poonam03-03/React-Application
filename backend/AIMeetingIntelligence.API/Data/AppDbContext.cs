using Microsoft.EntityFrameworkCore;
using AIMeetingIntelligence.API.Models;

namespace AIMeetingIntelligence.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<ActionItem> ActionItems => Set<ActionItem>();
}