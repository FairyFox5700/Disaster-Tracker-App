using DisasterTrackerApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisasterTrackerApp.Dal;

public class DisasterTrackerContext:DbContext
{
    public DisasterTrackerContext(DbContextOptions<DisasterTrackerContext> options)
        :base(options)
    {
        
    }

    public virtual DbSet<CalendarEvent> CalendarEvents { get; set; } = default!;
    public virtual DbSet<DisasterEvent> DisasterEvent { get; set; } = default!;
    public virtual DbSet<GoogleUser> GoogleUsers { get; set; } = default!;
    public virtual DbSet<GoogleCalendar> Calendars { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<GoogleUser>()
            .HasKey(e => e.UserId);

        modelBuilder.Entity<GoogleCalendar>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<GoogleCalendar>()
            .HasIndex(e => e.GoogleCalendarId)
            .IsUnique(true);


        modelBuilder.Entity<CalendarEvent>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<CalendarEvent>()
            .HasIndex(e => e.GoogleEventId)
            .IsUnique(true);

        modelBuilder.Entity<DisasterEvent>()
            .HasKey(e => e.Id);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditableEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateAuditableEntities();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditableEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void UpdateAuditableEntities()
    {
        var added = ChangeTracker.Entries<IAuditable>().Where(k => k.State == EntityState.Added).ToList();

        added.ForEach(e =>
        {
            e.Property(x => x.CreatedAt).CurrentValue = DateTimeOffset.UtcNow;
            e.Property(x => x.CreatedAt).IsModified = true;
        });

        var modified = ChangeTracker.Entries<IAuditable>().Where(entry => entry.State == EntityState.Modified).ToList();

        modified.ForEach(entry =>
        {
            entry.Property(x => x.UpdatedAt).CurrentValue = DateTimeOffset.UtcNow;
            entry.Property(x => x.UpdatedAt).IsModified = true;

            entry.Property(x => x.CreatedAt).CurrentValue = entry.Property(x => x.CreatedAt).OriginalValue;
            entry.Property(x => x.CreatedAt).IsModified = false;
        });

    }
}