using Server.Core.Models;

namespace Server.Core.Abstractions;

public interface ITokenManager
{
    string GenerateToken(User user);
}