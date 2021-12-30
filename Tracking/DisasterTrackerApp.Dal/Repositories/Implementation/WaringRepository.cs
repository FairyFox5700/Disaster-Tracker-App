using System.Linq.Expressions;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class WaringRepository : IWaringRepository
{
    private readonly DisasterTrackerContext _context;

    public WaringRepository(DisasterTrackerContext context)
    {
        _context = context;
    }

    public async Task<Warning?> GetWarningById(Guid waningId)
    {
        return await _context.Warnings.FindAsync(waningId);
    }

    public async Task<List<Warning>> GetAllWarningsFiltered(Expression<Func<Warning, bool>> predicate)
    {
        return await _context.Warnings
            .Where(predicate)
            .ToListAsync();
    }

    public void DeleteWarningsRange(List<Warning> warnings)
    {
        _context.Warnings.RemoveRange(warnings);
    }
}