using Server.Core.Models;
using Server.Persistence;
using Server.Persistence.Repositories;
using Domain.Exceptions;

namespace Server.Test.Persistence.Repositories;

public class SubjectsRepositoryTest
{
    private SchedulerDbContext dbContext;
    private SubjectsRepository repository;
    [SetUp]
    public void Setup()
    {
        var options = DataGenerator.CreateNewInMemoryDatabase();
        dbContext = new SchedulerDbContext(options);
        repository = new SubjectsRepository(dbContext);
    }
    [TearDown]
    public void TearDown()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
    [Theory]
    [TestCase("John", "John", 1)]
    [TestCase("John", "Jack", 0)]
    public async Task GetAllAsync_ReturnsSubjectsByUsername
        (string givenUsername, string expectedUsername, int expectedCount)
    {
        var id = 11;
        var subject = new Subject
        {
            Count = 0, Hours = 0, Id = id, Minutes = 0, Name = "", Type = 0, User = new User()
            {
                Id = 0, Password = "", Username = givenUsername
            }
        };
        await dbContext.AddAsync(subject);
        await dbContext.SaveChangesAsync();

        var results = await repository.GetAllAsync(expectedUsername);
        var itemsCount = results.Items.Count;
        Assert.That(itemsCount, Is.EqualTo(expectedCount));
    }
    [Test]
    public async Task GetByIdAsync_WithCorrectIdAndUsername_ReturnsSubject()
    {
        var id = 11;
        var givenName = "";
        var subject = DataGenerator.CreateTestSubject(id, givenName);
        await dbContext.AddAsync(subject);
        await dbContext.SaveChangesAsync();
        
        var result = await repository.GetByIdAsync(id, givenName);
        Assert.That(result, Is.Not.Null);
    }
    [Test]
    public async Task GetByIdAsync_WithCorrectIdAndWrongUsername_ThrowsNotFoundException()
    {
        var id = 11;
        var givenName = "John";
        var wrongName = "User2";
        var subject = DataGenerator.CreateTestSubject(id, givenName);
        await dbContext.AddAsync(subject);
        await dbContext.SaveChangesAsync();

        Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.GetByIdAsync(id, wrongName);
        });
    }
    [Test]
    public async Task GetByIdAsync_WithWrongIdAndCorrectUsername_ThrowsNotFoundException()
    {
        var id = 11;
        var wrongId = 12;
        var givenName = "";
        var subject = DataGenerator.CreateTestSubject(id, givenName);
        await dbContext.AddAsync(subject);
        await dbContext.SaveChangesAsync();

        Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.GetByIdAsync(wrongId, givenName);
        });
    }
    [Test]
    public async Task AddAsync_AddsSubject()
    {
        var id = 11;
        var givenName = "";
        var subject = DataGenerator.CreateTestSubject(id, givenName);
        await repository.AddAsync(subject);
        await dbContext.SaveChangesAsync();

        var foundSubject = await dbContext.Subjects.FindAsync(id);
        Assert.That(foundSubject, Is.Not.Null);
    }
    [Test]
    public async Task Update_UpdatesSubject()
    {
        var id = 11;
        var givenName = "";
        var subject = DataGenerator.CreateTestSubject(id, givenName);
        await dbContext.AddAsync(subject);
        await dbContext.SaveChangesAsync();

        var expectedUrl = "http://google.com";
        subject.Url = expectedUrl;
        repository.Update(subject);
        await dbContext.SaveChangesAsync();

        var foundSubject = await dbContext.Subjects.FindAsync(id);
        var actualUrl = foundSubject?.Url;
        Assert.That(actualUrl, Is.EqualTo(expectedUrl));
    }
    [Test]
    public async Task Remove_DeletesSubject()
    {
        var id = 11;
        var givenName = "";
        var subject = DataGenerator.CreateTestSubject(id, givenName);
        await dbContext.AddAsync(subject);
        await dbContext.SaveChangesAsync();

        var expectedUrl = "http://google.com";
        subject.Url = expectedUrl;
        repository.Remove(subject);
        await dbContext.SaveChangesAsync();

        var foundSubject = await dbContext.Subjects.FindAsync(id);
        Assert.That(foundSubject, Is.Null);
    }

}