using System.Linq.Expressions;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class DisasterEventRepository : IDisasterEventRepository
{
    private readonly DisasterTrackerContext _context;

    public DisasterEventRepository(DisasterTrackerContext context)
    {
        _context = context;
    }

    public async Task<DisasterEvent?> GetDisasterEventByExternalId(string externalId)
    {
        return await _context.DisasterEvent.FirstOrDefaultAsync(e=>e.ExternalApiId==externalId);
    }

    public async Task<List<DisasterEvent>> GetDisasterEventsFiltered(Expression<Func<DisasterEvent,bool>> predicate)
    {
        return await _context.DisasterEvent.Where(predicate).ToListAsync();
    }
}