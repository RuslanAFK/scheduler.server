using Domain.Exceptions;
using Server.Persistence;
using Server.Persistence.Repositories;

namespace Server.Test.Persistence.Repositories;

public class UnitOfWorkTest
{
    private SchedulerDbContext dbContext;
    private UnitOfWork unitOfWork;

    [SetUp]
    public void Setup()
    {
        var options = DataGenerator.CreateNewInMemoryDatabase();
        dbContext = new SchedulerDbContext(options);
        unitOfWork = new UnitOfWork(dbContext);
    }
    [TearDown]
    public void TearDown()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
    [Test]
    public void CompleteAsyncOrThrowIfNotCompleted_NoOperation_ThrowsOPerationNotSuccessful()
    {
        Assert.ThrowsAsync<OperationNotSuccessfulException>(async () =>
        {
            await unitOfWork.CompleteAsyncOrThrowIfNotCompleted();
        });
    }
    [Test]
    public async Task CompleteAsyncOrThrowIfNotCompleted_Passes()
    {
        var id = 11;
        var givenName = "";
        var subject = DataGenerator.CreateTestSubject(id, givenName);
        dbContext.Add(subject);
        await unitOfWork.CompleteAsyncOrThrowIfNotCompleted();
        Assert.Pass();
    }
}