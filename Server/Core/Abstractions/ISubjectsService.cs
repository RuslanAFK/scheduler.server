using Server.Core.Models;

namespace Server.Core.Abstractions;

public interface ISubjectsService
{
    Task<ListResponse<Subject>> GetAsync(string username);
    Task<Subject?> GetByIdAsync(int id, string username);
    Task<bool> CreateAsync(Subject subject, string username);
    Task<bool> UpdateAsync(Subject subject);
    Task<bool> RemoveAsync(Subject subject);
}