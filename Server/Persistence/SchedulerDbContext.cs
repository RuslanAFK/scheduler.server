using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Server.Core.Models;

namespace Server.Persistence;

public class SchedulerDbContext : DbContext
{
    public DbSet<Subject> Subjects { get; set; }
    public  DbSet<User> Users { get; set; }
    
    public SchedulerDbContext(DbContextOptions<SchedulerDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
