using Domain.Exceptions;

namespace Server.Persistence.Repositories;

public abstract class BaseRepository<TEntity>
{
    protected readonly SchedulerDbContext _context;

    protected BaseRepository(SchedulerDbContext context)
    {
        _context = context;
    }
    protected TEntity ReturnOrThrowIfDoesNotExist(TEntity? item, string propertyName)
    {
        if (item is null)
            throw new EntityNotFoundException(typeof(TEntity), propertyName);
        return item;
    }
    protected async Task AddAsync(TEntity item)
    {
        await _context.AddAsync(item);
    }
}