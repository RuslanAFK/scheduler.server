using Domain.Exceptions;
using Server.Persistence.Repositories;
using Server.Persistence;

namespace Server.Test.Persistence.Repositories;

public class UsersRepositoryTest
{
    private SchedulerDbContext dbContext;
    private UsersRepository repository;

    [SetUp]
    public void Setup()
    {
        var options = DataGenerator.CreateNewInMemoryDatabase();
        dbContext = new SchedulerDbContext(options);
        repository = new UsersRepository(dbContext);
    }
    [TearDown]
    public void TearDown()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [Test]
    public async Task RegisterAsync_NoDuplicate_AddsNewUser()
    {
        var user = DataGenerator.CreateTestUser();
        await repository.RegisterAsync(user);

        var id = user.Id;
        var foundUser = await dbContext.Users.FindAsync(id);
        Assert.That(foundUser, Is.Not.Null);
    }
    [Test]
    public async Task RegisterAsync_IfDuplicate_ThrowsAlreadyExists()
    {
        var username = "Name";
        var user = DataGenerator.CreateTestUser(name: username);
        dbContext.Add(user);
        await dbContext.SaveChangesAsync();

        Assert.ThrowsAsync<EntityAlreadyExistsException>(async() =>
        {
            await repository.RegisterAsync(user);
        });
    }
    [Test]
    public async Task GetByUsernameAsync_Exists_ReturnsUser()
    {
        var username = "Name";
        var user = DataGenerator.CreateTestUser(name: username);
        dbContext.Add(user);
        await dbContext.SaveChangesAsync();

        var foundUser = await repository.GetByUsernameAsync(username);
        Assert.That(foundUser, Is.Not.Null);
    }
    [Test]
    public async Task GetByUsernameAsync_DoesNotExist_ThrowsNotFound()
    {
        var username = "Name";
        Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.GetByUsernameAsync(username);
        });
    }
}