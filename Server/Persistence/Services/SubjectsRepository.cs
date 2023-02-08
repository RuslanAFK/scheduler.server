using Microsoft.EntityFrameworkCore;
using Server.Core.Models;
using Server.Core.Services;
using Server.Extensions;

namespace Server.Persistence.Services;

public class SubjectsRepository : ISubjectsRepository
{
    private readonly SchedulerDbContext _context;

    public SubjectsRepository(SchedulerDbContext context)
    {
        _context = context;
    }

    public async Task<ListResponse<Subject>> GetAsync(QueryObject queryObject, string username)
    {
        var subjects = _context.Subjects
            .Include(s => s.User)
            .ApplyAuthFiltering(username)
            .ApplySearching(queryObject);
        var response = new ListResponse<Subject>()
        {
            Count = subjects.Count(),
            Items = await subjects.ApplyPagination(queryObject, 4).ToListAsync()
        };
        return response;
    }

    public async Task<Subject?> GetByIdAsync(int id, string username)
    {
        return await _context.Subjects
            .Include(s => s.User)
            .ApplyAuthFiltering(username)
            .SingleOrDefaultAsync(s => s.Id == id);
    }

    public async Task CreateAsync(Subject subject)
    {
        await _context.Subjects.AddAsync(subject);
    }

    public void Update(Subject subject)
    {
        _context.Subjects.Update(subject);
    }

    public void Remove(Subject subject)
    {
        _context.Subjects.Remove(subject);
    }
}