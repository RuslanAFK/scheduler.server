using Domain.Exceptions;
using Server.Persistence.Repositories;

namespace Server.Test.Persistence.Repositories;

public class PasswordManagerTest
{
    private PasswordManager passwordManager;
    [SetUp]
    public void Setup()
    {
        passwordManager = new PasswordManager();
    }
    [Test]
    public void SecureUser_VerifyWithBCrypt()
    {
        var textPassword = "password";
        var user = DataGenerator.CreateTestUser(password: textPassword);
        passwordManager.SecureUser(user);
        var isHashingCorrect = BCrypt.Net.BCrypt.Verify(textPassword, user.Password);
        Assert.That(isHashingCorrect, Is.True);
    }
    
    [Test]
    public void ThrowExceptionIfWrongPassword_WithCorrectPassword_Passes()
    {
        var password = "password";
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);
        passwordManager.ThrowExceptionIfWrongPassword(password, hashed);
        Assert.Pass();
    }
    [Test]
    public void ThrowExceptionIfWrongPassword_WithWrongPassword_ThrowsWrongPasswordException()
    {
        var password = "password";
        var wrongPassword = "wrong";
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);
        Assert.Throws<WrongPasswordException>(() => 
            passwordManager.ThrowExceptionIfWrongPassword(wrongPassword, hashed));
    }
    [Test]
    public void ThrowExceptionIfWrongPassword_WithNotHashedValue_PasswordNotParseableException()
    {
        var password = "password";
        var notHashedPassword = "not hashed at all";
        Assert.Throws<PasswordNotParseableException>(() =>
        {
            passwordManager.ThrowExceptionIfWrongPassword(password, notHashedPassword);
        });
    }
}