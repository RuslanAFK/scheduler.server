using Server.Core.Models;

namespace Server.Core.Abstractions;

public interface IUsersRepository
{
    Task RegisterAsync(User inputUser);
    Task<User> GetByUsernameAsync(string username);
}