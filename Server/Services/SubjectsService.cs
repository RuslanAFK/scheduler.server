using Server.Core.Abstractions;
using Server.Core.Models;

namespace Server.Services;

public class SubjectsService : ISubjectsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISubjectsRepository _subjectsRepository;
    private readonly IUsersRepository _usersRepository;

    
    public Task<ListResponse<Subject>> GetAsync(string username)
    {
        return _subjectsRepository.GetAsync(username);
    }

    public Task<Subject?> GetByIdAsync(int id, string username)
    {
        return _subjectsRepository.GetByIdAsync(id, username);
    }

    public async Task<bool> CreateAsync(Subject subject, string username)
    {
       var user  = await _usersRepository.GetUserByUsername(username);
       if (user == null)
           return false;
       subject.User = user;
       await _subjectsRepository.CreateAsync(subject);
       return await IsCompleted();
    }

    public async Task<bool> UpdateAsync(Subject subject)
    {
        _subjectsRepository.Update(subject);
        return await IsCompleted();
    }

    public async Task<bool> RemoveAsync(Subject subject)
    {
        _subjectsRepository.Remove(subject);
        return await IsCompleted();
    }

    public SubjectsService(IUnitOfWork unitOfWork, ISubjectsRepository subjectsRepository, IUsersRepository usersRepository)
    {
        _unitOfWork = unitOfWork;
        _subjectsRepository = subjectsRepository;
        _usersRepository = usersRepository;
    }
    private async Task<bool> IsCompleted()
    {
        return await _unitOfWork.CompleteAsync() > 0;
    }
    
}