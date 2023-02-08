using Microsoft.EntityFrameworkCore;
using Server.Core.Models;
using Server.Core.Services;

namespace Server.Persistence.Services;

public class UsersRepository : IUsersRepository
{
    private readonly SchedulerDbContext _context;

    public UsersRepository(SchedulerDbContext context)
    {
        _context = context;
    }
    
    public void Signup(User userToCreate)
    {
        _context.Users.Add(userToCreate);
    }

    public async Task<User?> CheckCredentialsAsync(User userToLogin)
    {
        var userFound = await _context.Users.SingleOrDefaultAsync(user =>
            user.Username == userToLogin.Username);
        return userFound;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        var userFound = await _context.Users.SingleOrDefaultAsync(user =>
            user.Username == username);
        return userFound;
    }
}