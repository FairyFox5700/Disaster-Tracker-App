using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface IWaringRepository
{
    Task<Warning?> GetWarningById(Guid waningId);
    Task<List<Warning>> GetAllWarningsFiltered(Expression<Func<Warning, bool>> predicate);
    void DeleteWarningsRange(List<Warning> warnings);
}