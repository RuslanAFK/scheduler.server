using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Controllers.Resources;
using Server.Core.Models;
using Server.Core.Services;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    private readonly IUsersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITokenManager _tokenManager;

    public UsersController(IUsersRepository repository, IUnitOfWork unitOfWork, IMapper mapper,
        ITokenManager tokenManager)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenManager = tokenManager;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginResource loginResource)
    {
        var user = _mapper.Map<LoginResource, User>(loginResource);
        var foundUser = await _repository.CheckCredentialsAsync(user);
        if (foundUser == null)
            return NotFound();
        if (!BCrypt.Net.BCrypt.Verify(user.Password, foundUser.Password)) 
            return BadRequest("Provided incorrect password.");
        var token = _tokenManager.GenerateToken(foundUser);
        return Ok(new AuthResult
        {
            Id = foundUser.Id,
            Username = foundUser.Username,
            Token = token,
        });
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterResource registerResource)
    {
        registerResource.Password = BCrypt.Net.BCrypt.HashPassword(registerResource.Password);
        try
        {  
            var userToCreate = _mapper.Map<RegisterResource, User>(registerResource);
            _repository.Signup(userToCreate);
            var createSuccessful = await _unitOfWork.CompleteAsync();
            if (createSuccessful > 0)
                return NoContent();
            return BadRequest();
        }
        catch (DbUpdateException e)
        {
            var inner = e.InnerException;
            return BadRequest(inner==null ? e.Message : inner.Message);
        }
    }
}
