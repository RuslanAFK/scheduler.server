using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Server.Core.Abstractions;
using Server.Core.Models;

namespace Server.Persistence.Repositories;

public class TokenManager : ITokenManager
{
    private readonly IConfiguration _configuration;

    public TokenManager(IConfiguration config)
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
}