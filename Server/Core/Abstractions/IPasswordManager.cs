using Server.Core.Models;

namespace Server.Core.Abstractions;

public interface IPasswordManager
{
    void SecureUser(User user);
    void ThrowExceptionIfWrongPassword(string realPassword, string hashedPassword);
}