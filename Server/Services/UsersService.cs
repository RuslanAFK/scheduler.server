using Server.Core.Abstractions;
using Server.Core.Models;
using System.Security.Claims;

namespace Server.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IClaimsManager _claimsManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordManager _passwordManager;

    public UsersService(IUsersRepository usersRepository, IUnitOfWork unitOfWork, IClaimsManager claimsManager, IPasswordManager passwordManager)
    {
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _claimsManager = claimsManager;
        _passwordManager = passwordManager;
    }
    public async Task RegisterAsync(User userToCreate)
    {
        _passwordManager.SecureUser(userToCreate);
        await _usersRepository.RegisterAsync(userToCreate);
        await _unitOfWork.CompleteAsyncOrThrowIfNotCompleted();
    }

    public async Task<AuthResult> GetAuthResultAsync(User userToLogin)
    {
        var username = userToLogin.Username;
        var foundUser = await _usersRepository.GetByUsernameAsync(username);
        _passwordManager.ThrowExceptionIfWrongPassword(userToLogin.Password, foundUser.Password);
        var token = _claimsManager.GenerateToken(foundUser);
        var authResult = new AuthResult(foundUser, token);
        return authResult;
    }
    public string GetUsername(ClaimsPrincipal? claimsPrincipal)
    {
        return _claimsManager.GetUsernameOrThrow(claimsPrincipal);
    }
}