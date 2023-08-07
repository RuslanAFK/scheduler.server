using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Domain.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Server.Core.Abstractions;
using Server.Core.Models;

namespace Server.Persistence.Repositories;

public class ClaimsManager : IClaimsManager
{
    private readonly IConfiguration _configuration;

    public ClaimsManager(IConfiguration config)
    {
        _configuration = config;
    }
    
    public string GenerateToken(User user)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(_configuration["Jwt:PrivateKey"]), out _);
        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        };
        var jwtDate = DateTime.Now;
        
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.UniqueName, user.Username)
        };
        var jwt = new JwtSecurityToken(claims: claims, notBefore: jwtDate, expires: jwtDate.AddMinutes(60), 
            signingCredentials: signingCredentials);
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    public string GetUsernameOrThrow(ClaimsPrincipal? claimsPrincipal)
    {
        var identity = claimsPrincipal?.Identity;
        var username = identity?.Name;
        var authenticated = identity?.IsAuthenticated ?? false;
        if (!authenticated)
            throw new UserNotAuthorizedException();
        if (username == null)
            throw new EntityNotFoundException(typeof(User), nameof(User.Username));
        return username;
    }
}