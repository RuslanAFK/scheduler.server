using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.Resources;
using Server.Core.Abstractions;
using Server.Core.Models;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUsersService _usersService;

    public UsersController(IMapper mapper, IUsersService usersService)
    {
        _mapper = mapper;
        _usersService = usersService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginResource loginResource)
    {
        var user = _mapper.Map<LoginResource, User>(loginResource);
        var authResult = await _usersService.GetAuthResultAsync(user);
        var result = _mapper.Map<AuthResult, AuthResultResource>(authResult);
        return Ok(result);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterResource registerResource)
    {
        var userToCreate = _mapper.Map<RegisterResource, User>(registerResource);
        await _usersService.RegisterAsync(userToCreate);
        return NoContent();
    }
}
