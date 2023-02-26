using Server.Core.Abstractions;

namespace Server.Persistence.Repositories;

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