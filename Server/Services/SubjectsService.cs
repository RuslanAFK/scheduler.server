using Server.Core.Abstractions;
using Server.Core.Models;
using Server.Extensions;

namespace Server.Services;

public class SubjectsService : ISubjectsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISubjectsRepository _subjectsRepository;
    private readonly IUsersRepository _usersRepository;

    public SubjectsService(IUnitOfWork unitOfWork, ISubjectsRepository subjectsRepository, IUsersRepository usersRepository)
    {
        _unitOfWork = unitOfWork;
        _subjectsRepository = subjectsRepository;
        _usersRepository = usersRepository;
    }
    public async Task<ListResponse<Subject>> GetAllAsync(string username)
    {
        return await _subjectsRepository.GetAllAsync(username);
    }

    public Task<Subject> GetByIdAsync(int id, string username)
    {
        return _subjectsRepository.GetByIdAsync(id, username);
    }

    public async Task CreateAsync(Subject subject, string username)
    {
       var user  = await _usersRepository.GetByUsernameAsync(username);
       AssignUser(subject, user);
       await _subjectsRepository.AddAsync(subject);
       await _unitOfWork.CompleteAsyncOrThrowIfNotCompleted();
    }

    private void AssignUser(Subject subject, User user)
    {
        subject.User = user;
    }
    public async Task UpdateAsync(Subject subject)
    {
        _subjectsRepository.Update(subject);
        await _unitOfWork.CompleteAsyncOrThrowIfNotCompleted();
    }

    public async Task RemoveAsync(Subject subject)
    {
        _subjectsRepository.Remove(subject);
        await _unitOfWork.CompleteAsyncOrThrowIfNotCompleted();
    }
}