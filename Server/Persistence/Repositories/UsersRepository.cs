using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Server.Core.Abstractions;
using Server.Core.Models;

namespace Server.Persistence.Repositories;

public class UsersRepository : BaseRepository<User>, IUsersRepository
{
    public UsersRepository(SchedulerDbContext context) : base(context)
    {
    }
    public async Task RegisterAsync(User inputUser)
    {
        var username = inputUser.Username;
        var user = await ReturnByUsernameAsync(username);
        ThrowIfExists(user);
        await AddAsync(inputUser);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var user = await ReturnByUsernameAsync(username);
        var notNullUser = ReturnOrThrowIfDoesNotExist(user, nameof(User.Username));
        return notNullUser;
    }
    private async Task<User?> ReturnByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(user =>
            user.Username == username);
    }
    private void ThrowIfExists(User? user)
    {
        if (user is not null)
            throw new EntityAlreadyExistsException(typeof(User), nameof(User.Username));
    }
}