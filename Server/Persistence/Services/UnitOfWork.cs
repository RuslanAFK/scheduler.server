using Server.Core.Services;

namespace Server.Persistence.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly SchedulerDbContext _context;

    public UnitOfWork(SchedulerDbContext context)
    {
        _context = context;
    }
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
}