using BCrypt.Net;
using Domain.Exceptions;
using Server.Core.Abstractions;
using Server.Core.Models;

namespace Server.Persistence.Repositories;

public class PasswordManager : IPasswordManager
{
    public void SecureUser(User user)
    {
        var textPassword = user.Password;
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(textPassword);
        user.Password = hashedPassword;
    }
    public void ThrowExceptionIfWrongPassword(string realPassword, string hashedPassword)
    {
        try
        {
            var passwordCorrect =
                BCrypt.Net.BCrypt.Verify(realPassword, hashedPassword);
            if (!passwordCorrect)
                throw new WrongPasswordException();
        }
        catch (SaltParseException)
        {
            throw new PasswordNotParseableException();
        }
    }
}