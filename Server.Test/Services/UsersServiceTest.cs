using System.Security.Claims;
using Server.Core.Abstractions;
using Server.Core.Models;
using Server.Services;

namespace Server.Test.Services;

public class UsersServiceTest
{
    private IUsersRepository usersRepository;
    private IClaimsManager claimsManager;
    private IUnitOfWork unitOfWork;
    private IPasswordManager passwordManager;
    private UsersService service;
    [SetUp]
    public void Setup()
    {
        usersRepository = A.Fake<IUsersRepository>();
        unitOfWork = A.Fake<IUnitOfWork>();
        claimsManager = A.Fake<IClaimsManager>();
        passwordManager = A.Fake<IPasswordManager>();
        service = new UsersService(usersRepository, unitOfWork, claimsManager, passwordManager);
    }
    [Test]
    public async Task RegisterAsync_Calls3Methods()
    {
        var user = A.Dummy<User>();
        await service.RegisterAsync(user);
        A.CallTo(() => passwordManager.SecureUser(A<User>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => usersRepository.RegisterAsync(A<User>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => unitOfWork.CompleteAsyncOrThrowIfNotCompleted()).MustHaveHappenedOnceExactly();
    }
    [Test]
    public async Task GetAuthResult_ReturnsAuthResult()
    {
        var user = DataGenerator.CreateTestUser(1, "name", "password");
        var authResult = await service.GetAuthResultAsync(user);
        Assert.That(authResult, Is.InstanceOf<AuthResult>());
    }
    [Test]
    public void GetUsername_CallsGetUsernameOrThrow()
    {
        var claimsPrincipal = A.Dummy<ClaimsPrincipal?>();
        service.GetUsername(claimsPrincipal);
        A.CallTo(() => claimsManager.GetUsernameOrThrow(A<ClaimsPrincipal?>._)).MustHaveHappenedOnceExactly();
    }
}