using Server.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.Resources;
using Server.Core.Abstractions;

namespace Server.Test.Controllers;

public class UsersControllersTest
{
    private UsersController usersController;

    [SetUp]
    public void Setup()
    {
        var mapper = A.Fake<IMapper>();
        var usersService = A.Fake<IUsersService>();
        usersController = new UsersController(mapper, usersService);
    }

    [Test]
    public async Task Login_ReturnValueIsAuthResultResource()
    {
        var resource = A.Dummy<LoginResource>();
        var results = await usersController.Login(resource) as OkObjectResult;
        var resultsValue = results?.Value;
        Assert.That(resultsValue, Is.InstanceOf<AuthResultResource>());
    }
    [Test]
    public async Task Register_ReturnsNoContent()
    {
        var resource = A.Dummy<RegisterResource>();
        var results = await usersController.Register(resource);
        Assert.That(results, Is.InstanceOf<NoContentResult>());
    }
}