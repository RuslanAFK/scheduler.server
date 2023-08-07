using Server.Core.Abstractions;
using Server.Core.Models;
using Server.Services;

namespace Server.Test.Services;

public class SubjectsServiceTest
{
    private IUnitOfWork unitOfWork;
    private ISubjectsRepository subjectsRepository;
    private IUsersRepository usersRepository;
    private SubjectsService service;

    [SetUp]
    public void Setup()
    {
        unitOfWork = A.Fake<IUnitOfWork>();
        subjectsRepository = A.Fake<ISubjectsRepository>();
        usersRepository = A.Fake<IUsersRepository>();
        service = new SubjectsService(unitOfWork, subjectsRepository, usersRepository);
    }
    [Test]
    public async Task GetAllAsync_ReturnsListResponseOfSubject()
    {
        var username = A.Dummy<string>();
        var results = await service.GetAllAsync(username);
        Assert.That(results, Is.InstanceOf<ListResponse<Subject>>());
    }
    [Test]
    public async Task GetByIdAsync_ReturnsSubject()
    {
        var username = A.Dummy<string>();
        var id = A.Dummy<int>();
        var results = await service.GetByIdAsync(id, username);
        Assert.That(results, Is.InstanceOf<Subject>());
    }
    [Test]
    public async Task CreateAsync_Calls3Methods()
    {
        var subject = A.Dummy<Subject>();
        var username = A.Dummy<string>();
        await service.CreateAsync(subject, username);
        A.CallTo(() => usersRepository.GetByUsernameAsync(A<string>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => subjectsRepository.AddAsync(A<Subject>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => unitOfWork.CompleteAsyncOrThrowIfNotCompleted()).MustHaveHappenedOnceExactly();
    }
    [Test]
    public async Task UpdateAsync_Calls2Methods()
    {
        var subject = A.Dummy<Subject>();
        await service.UpdateAsync(subject);
        A.CallTo(() => subjectsRepository.Update(A<Subject>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => unitOfWork.CompleteAsyncOrThrowIfNotCompleted()).MustHaveHappenedOnceExactly();
    }
    [Test]
    public async Task RemoveAsync_Calls2Methods()
    {
        var subject = A.Dummy<Subject>();
        await service.RemoveAsync(subject);
        A.CallTo(() => subjectsRepository.Remove(A<Subject>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => unitOfWork.CompleteAsyncOrThrowIfNotCompleted()).MustHaveHappenedOnceExactly();
    }
}