using Server.Core.Models;

namespace Server.Core.Services;

public interface ITokenManager
{
    string GenerateToken(User user);
}