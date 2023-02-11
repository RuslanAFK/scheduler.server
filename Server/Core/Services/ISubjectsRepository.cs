using Server.Core.Models;

namespace Server.Core.Services;

public interface ISubjectsRepository
{
    Task<ListResponse<Subject>> GetAsync(string username);
    Task<Subject?> GetByIdAsync(int id, string username);
    Task CreateAsync(Subject subject);
    void Update(Subject subject);
    void Remove(Subject subject);
}