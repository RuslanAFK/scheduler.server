using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Controllers.Resources;
using Server.Core.Abstractions;
using Server.Core.Models;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectsController : Controller
{
    private readonly IMapper _mapper;
    private readonly ISubjectsService _subjectsService;

    public SubjectsController(IMapper mapper, ISubjectsService subjectsService)
    {
        _mapper = mapper;
        _subjectsService = subjectsService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetSubjects()
    {
        var username = GetUsername();
        if (username == null)
            return BadRequest();
        var subjects = await _subjectsService.GetAsync(username);
        var res = 
            _mapper.Map<ListResponse<Subject>, ListResponseResource<GetSubjectsResource>>(subjects);
        return Ok(res);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetSubject(int id)
    {
        var username = GetUsername();
        if (username == null)
            return BadRequest();
        var subject = await _subjectsService.GetByIdAsync(id, username);
        if (subject == null)
            return NotFound();
        var res = _mapper.Map<Subject, GetSingleSubjectResource>(subject);
        return Ok(res);
    }
        
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateSubjectResource resource)
    {
        var username = GetUsername();
        if (username == null)
            return BadRequest();
        try
        {
            var subject = _mapper.Map<CreateSubjectResource, Subject>(resource);
            var success = await _subjectsService.CreateAsync(subject, username);
            if (success)
                return NoContent();
            return BadRequest();
        }
        catch (DbUpdateException e)
        {
            var inner = e.InnerException;
            return BadRequest(inner==null ? e.Message : inner.Message);
        }
    }
        
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(UpdateSubjectResource resource)
    {
        var username = GetUsername();
        if (username == null)
            return BadRequest();
        var subject = await _subjectsService.GetByIdAsync(resource.Id, username);
        if (subject == null)
            return NotFound();
        _mapper.Map(resource, subject);
        try
        {
            var success = await _subjectsService.UpdateAsync(subject);
            if (success)
                return NoContent();
            return BadRequest();
        }
        catch (DbUpdateException e)
        {
            var inner = e.InnerException;
            return BadRequest(inner==null ? e.Message : inner.Message);
        }
    }
        
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var username = GetUsername();
        if (username == null)
            return BadRequest();
        var subject = await _subjectsService.GetByIdAsync(id, username);
        if (subject == null)
            return NotFound();
        try
        {
            var success = await _subjectsService.RemoveAsync(subject);
            if (success)
                return NoContent();
            return BadRequest();
        }
        catch (DbUpdateException e)
        {
            var inner = e.InnerException;
            return BadRequest(inner==null ? e.Message : inner.Message);
        }
    }
    private string? GetUsername()
    {
        return (HttpContext.User.Identity as ClaimsIdentity)?.Name;
    }
}