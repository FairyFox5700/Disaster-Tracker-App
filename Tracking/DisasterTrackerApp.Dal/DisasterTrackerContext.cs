using DisasterTrackerApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisasterTrackerApp.Dal;

public class DisasterTrackerContext:DbContext
{
    public DisasterTrackerContext(DbContextOptions<DisasterTrackerContext> options):base(options)
    {
        
    }
    
    public  virtual  DbSet<CalendarEvent> CalendarEvents { get; set; }
    public  virtual  DbSet<DisasterEvent> DisasterEvent { get; set; }
    public  virtual  DbSet<Warning> Warnings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}