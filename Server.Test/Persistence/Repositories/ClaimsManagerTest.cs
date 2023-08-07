using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Server.Persistence.Repositories;

namespace Server.Test.Persistence.Repositories;

public class ClaimsManagerTest
{
    private IConfiguration configuration;
    private ClaimsManager claimsManager;
    [SetUp]
    public void Setup()
    {
        configuration = A.Fake<IConfiguration>();
        claimsManager = new ClaimsManager(configuration);
    }

    [Test]
    public void GenerateToken_WithCorrectPrivateKey_ReturnTokenWithClaimsUniqueName()
    {
        var user = DataGenerator.CreateTestUser(1, "John", "none");
        configuration["Jwt:PrivateKey"] = DataGenerator.GetPrivateKey();
        var token = claimsManager.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);

        IList<Claim> claims = (IList<Claim>)decodedToken.Claims;
        var nameClaim = claims[0];
        Assert.That(nameClaim.Type, Is.EqualTo(JwtRegisteredClaimNames.UniqueName));
        Assert.That(nameClaim.Value, Is.EqualTo(user.Username));
    }
    [Test]
    public void GetUsernameOrThrow_WithNeededClaimAndCtor_ReturnsUsername()
    {
        var username = "Stephen";
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.UniqueName, username)
        };
        var identity = new ClaimsIdentity(claims, "Bearer",
            JwtRegisteredClaimNames.UniqueName, "");
        var principal = new ClaimsPrincipal(identity);
        var foundUsername = claimsManager.GetUsernameOrThrow(principal);
        Assert.That(foundUsername == username);
    }
    [Test]
    public void GetUsernameOrThrow_WithFakeIdentity_ThrowsUserNotAuthorizedException()
    {
        var claimsPrincipal = A.Fake<ClaimsPrincipal>();
        Assert.Throws<UserNotAuthorizedException>(() =>
        {
            claimsManager.GetUsernameOrThrow(claimsPrincipal);
        });
    }
    [Test]
    public void GetUsernameOrThrow_WithoutNeededClaimAndCtor_ThrowsUserNotFoundException()
    {
        var claims = new List<Claim>();
        var authenticationType = "Bearer";
        var identity = new ClaimsIdentity(claims, authenticationType);
        var principal = new ClaimsPrincipal(identity);
        Assert.Throws<EntityNotFoundException>(() =>
        {
            claimsManager.GetUsernameOrThrow(principal);
        });
    }
}