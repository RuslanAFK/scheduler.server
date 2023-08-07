using Server.Core.Models;
using Server.Extensions;

namespace Server.Core.Abstractions;

public interface ISubjectsService
{
    Task<ListResponse<Subject>> GetAllAsync(string username);
    Task<Subject> GetByIdAsync(int id, string username);
    Task CreateAsync(Subject subject, string username);
    Task UpdateAsync(Subject subject);
    Task RemoveAsync(Subject subject);
}