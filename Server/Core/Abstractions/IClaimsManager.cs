using Server.Core.Models;
using System.Security.Claims;

namespace Server.Core.Abstractions;

public interface IClaimsManager
{
    string GenerateToken(User user);
    string GetUsernameOrThrow(ClaimsPrincipal? claimsPrincipal);
}