using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.Resources;
using Server.Core.Abstractions;
using Server.Core.Models;
using Server.Extensions;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectsController : Controller
{
    private readonly IMapper _mapper;
    private readonly ISubjectsService _subjectsService;
    private readonly IUsersService _userService;

    public SubjectsController(IMapper mapper, ISubjectsService subjectsService, IUsersService userService)
    {
        _mapper = mapper;
        _subjectsService = subjectsService;
        _userService = userService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetSubjects()
    {
        var username = _userService.GetUsername(HttpContext?.User);
        var studyWeek = await _subjectsService.GetAllAsync(username);
        var result = _mapper.Map<ListResponse<Subject>, ListResponseResource<GetSubjectsResource>>(studyWeek);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetSubject(int id)
    {
        var username = _userService.GetUsername(HttpContext?.User);
        var subject = await _subjectsService.GetByIdAsync(id, username);
        var result = _mapper.Map<Subject, GetSingleSubjectResource>(subject);
        return Ok(result);
    }
        
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateSubjectResource resource)
    {
        var username = _userService.GetUsername(HttpContext?.User);
        var subject = _mapper.Map<CreateSubjectResource, Subject>(resource);
        await _subjectsService.CreateAsync(subject, username);
        return NoContent();
    }
        
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(UpdateSubjectResource resource)
    {
        var username = _userService.GetUsername(HttpContext?.User);
        var subject = await _subjectsService.GetByIdAsync(resource.Id, username);
        _mapper.Map(resource, subject);
        await _subjectsService.UpdateAsync(subject);
        return NoContent();
    }
        
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var username = _userService.GetUsername(HttpContext?.User);
        var subject = await _subjectsService.GetByIdAsync(id, username);
        await _subjectsService.RemoveAsync(subject);
        return NoContent();
    }
}