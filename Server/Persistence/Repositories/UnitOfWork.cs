using Domain.Exceptions;
using Server.Core.Abstractions;

namespace Server.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SchedulerDbContext _context;

    public UnitOfWork(SchedulerDbContext context)
    {
        _context = context;
    }
    public async Task CompleteAsyncOrThrowIfNotCompleted()
    {
        var entries = await _context.SaveChangesAsync();
        if (entries <= 0)
            throw new OperationNotSuccessfulException();
    }
}