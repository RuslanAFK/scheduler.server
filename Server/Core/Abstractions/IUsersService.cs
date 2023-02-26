using Server.Core.Models;

namespace Server.Core.Abstractions;

public interface IUsersService
{
    Task<bool> RegisterAsync(User userToCreate);
    Task<AuthResult?> GetAuthResult(User userToLogin);
}