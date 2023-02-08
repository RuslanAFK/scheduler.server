using Server.Core.Models;

namespace Server.Core.Services;

public interface IUsersRepository
{
    void Signup(User userToCreate);
    Task<User?> CheckCredentialsAsync(User userToLogin);
    Task<User?> GetUserByUsername(string username);
}