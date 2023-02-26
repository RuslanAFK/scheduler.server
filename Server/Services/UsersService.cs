using Server.Core.Abstractions;
using Server.Core.Models;

namespace Server.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenManager _tokenManager;
    private readonly IUnitOfWork _unitOfWork;

    public UsersService(IUsersRepository usersRepository, IUnitOfWork unitOfWork, ITokenManager tokenManager)
    {
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _tokenManager = tokenManager;
    }
    public async Task<bool> RegisterAsync(User userToCreate)
    {
        _usersRepository.Signup(userToCreate);
        return await IsCompleted();
    }

    public async Task<AuthResult?> GetAuthResult(User userToLogin)
    {
        var foundUser = await _usersRepository.CheckCredentialsAsync(userToLogin);
        if (foundUser == null)
            return null;
        CheckPassword(userToLogin.Password, foundUser.Password);
        var token = _tokenManager.GenerateToken(foundUser);
        return new AuthResult
        {
            Id = foundUser.Id,
            Username = foundUser.Username,
            Token = token,
        };
    }

    private async Task<bool> IsCompleted()
    {
        return await _unitOfWork.CompleteAsync() > 0;
    }

    private void CheckPassword(string password, string hash)
    {
        if (!BCrypt.Net.BCrypt.Verify(password, hash))
            throw new InvalidDataException("Provided incorrect password.");
    }
    
}