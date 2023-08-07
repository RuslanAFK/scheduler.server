using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.Resources;
using Server.Controllers;
using Server.Core.Abstractions;

namespace Server.Test.Controllers;

public class SubjectsControllerTest
{
    private SubjectsController subjectsController;

    [SetUp]
    public void Setup()
    {
        var mapper = A.Fake<IMapper>();
        var usersService = A.Fake<IUsersService>();
        var subjectsService = A.Fake<ISubjectsService>();
        subjectsController = new SubjectsController(mapper, subjectsService, usersService);
    }

    [Test]
    public async Task GetSubjects_ReturnValueIsListResponseResourceOfGetSubjectsResource()
    {
        var results = await subjectsController.GetSubjects() as OkObjectResult;
        var resultsValue = results?.Value;
        Assert.That(resultsValue, Is.InstanceOf<ListResponseResource<GetSubjectsResource>>());
    }
    [Test]
    public async Task GetSubject_ReturnValueIsGetSingleSubjectResource()
    {
        var id = A.Dummy<int>();
        var results = await subjectsController.GetSubject(id) as OkObjectResult;
        var resultsValue = results?.Value;
        Assert.That(resultsValue, Is.InstanceOf<GetSingleSubjectResource>());
    }
    [Test]
    public async Task Create_ReturnsNoContent()
    {
        var resource = A.Dummy<CreateSubjectResource>();
        var results = await subjectsController.Create(resource);
        Assert.That(results, Is.InstanceOf<NoContentResult>());
    }
    [Test]
    public async Task Update_ReturnsNoContent()
    {
        var resource = A.Dummy<UpdateSubjectResource>();
        var results = await subjectsController.Update(resource);
        Assert.That(results, Is.InstanceOf<NoContentResult>());
    }
    [Test]
    public async Task Delete_ReturnsNoContent()
    {
        var id = A.Dummy<int>();
        var results = await subjectsController.Delete(id);
        Assert.That(results, Is.InstanceOf<NoContentResult>());
    }
}