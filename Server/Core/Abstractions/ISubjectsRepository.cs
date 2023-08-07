using Server.Core.Models;
using Server.Extensions;

namespace Server.Core.Abstractions;

public interface ISubjectsRepository
{
    Task<ListResponse<Subject>> GetAllAsync(string username);
    Task<Subject> GetByIdAsync(int id, string username);
    Task AddAsync(Subject subject);
    void Update(Subject subject);
    void Remove(Subject subject);
}