using DisasterTracker.Identity.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisasterTracker.Identity.DAL;

public class IdentityContext:DbContext
{
    public  virtual  DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {


    }

}