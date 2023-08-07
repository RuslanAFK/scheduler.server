using Server.Core.Models;
using System.Security.Claims;

namespace Server.Core.Abstractions;

public interface IUsersService
{
    Task RegisterAsync(User userToCreate);
    Task<AuthResult> GetAuthResultAsync(User userToLogin);
    string GetUsername(ClaimsPrincipal? claimsPrincipal);
}