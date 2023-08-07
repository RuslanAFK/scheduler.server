using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Server.Core.Abstractions;
using Server.Core.Models;
using Server.Extensions;

namespace Server.Persistence.Repositories;

public class SubjectsRepository : BaseRepository<Subject>, ISubjectsRepository
{
    public SubjectsRepository(SchedulerDbContext context) : base(context)
    {
    }
    public async Task<ListResponse<Subject>> GetAllAsync(string username)
    {
        var subjects = GetAllByUsernameAsync(username);
        var subjectsGrouping = await subjects.ApplySubjectGrouping()
            .ToDictionaryAsync(g => g.Key, g => g.Value);
        var response = new ListResponse<Subject>(subjectsGrouping);
        return response;
    }

    public async Task<Subject> GetByIdAsync(int id, string username)
    {
        var subject = await GetAllByUsernameAsync(username)
            .SingleOrDefaultAsync(s => s.Id == id);
        var notNullSubject = ReturnOrThrowIfDoesNotExist(subject, nameof(Subject.Id));
        return notNullSubject;
    }

    private IQueryable<Subject> GetAllByUsernameAsync(string username)
    {
        return _context.Subjects
            .Include(s => s.User)
            .ApplyAuthFiltering(username);
    }

    public new async Task AddAsync(Subject subject)
    {
        await base.AddAsync(subject);
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