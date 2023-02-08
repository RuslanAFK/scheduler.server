using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Controllers.Resources;
using Server.Core.Models;
using Server.Core.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : Controller
    {
        private IMapper _mapper;
        private IUsersRepository _usersRepository;
        private ISubjectsRepository _subjectsRepository;
        private IUnitOfWork _unitOfWork;
        private ITokenManager _tokenManager;

        public SubjectsController(IMapper mapper, IUsersRepository usersRepository, IUnitOfWork unitOfWork, 
            ISubjectsRepository subjectsRepository, ITokenManager tokenManager)
        {
            _mapper = mapper;
            _usersRepository = usersRepository;
            _unitOfWork = unitOfWork;
            _subjectsRepository = subjectsRepository;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Asymmetric")]
        public async Task<IActionResult> GetSubjects([FromQuery] QueryObject queryObject)
        {
            var username = (HttpContext.User.Identity as ClaimsIdentity)?.Name;
            if (username == null)
                return BadRequest();
            var subjects = await _subjectsRepository.GetAsync(queryObject, username);
            var res = 
                _mapper.Map<ListResponse<Subject>, ListResponseResource<GetSubjectResource>>(subjects);
            return Ok(res);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = "Asymmetric")]
        public async Task<IActionResult> GetSubject(int id)
        {
            var username = (HttpContext.User.Identity as ClaimsIdentity)?.Name;
            if (username == null)
                return BadRequest();
            var subject = await _subjectsRepository.GetByIdAsync(id, username);
            if (subject == null)
                return NotFound();
            var res = _mapper.Map<Subject, GetSubjectResource>(subject);
            return Ok(res);
        }
        
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Asymmetric")]
        public async Task<IActionResult> Create(CreateSubjectResource resource)
        {
            try
            {  
                var username = (HttpContext.User.Identity as ClaimsIdentity)?.Name;
                if (username == null)
                    return BadRequest();
                var subject = _mapper.Map<CreateSubjectResource, Subject>(resource);
                subject.User = await _usersRepository.GetUserByUsername(username);
                await _subjectsRepository.CreateAsync(subject);
                var success = await _unitOfWork.CompleteAsync();
                if (success > 0)
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
        [Authorize(AuthenticationSchemes = "Asymmetric")]
        public async Task<IActionResult> Update(UpdateSubjectResource resource)
        {
            try
            {  
                var username = (HttpContext.User.Identity as ClaimsIdentity)?.Name;
                if (username == null)
                    return BadRequest();
                var subject = await _subjectsRepository.GetByIdAsync(resource.Id, username);
                if (subject == null)
                    return NotFound();
                _mapper.Map(resource, subject);
                _subjectsRepository.Update(subject);
                var success = await _unitOfWork.CompleteAsync();
                if (success > 0)
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
        [Authorize(AuthenticationSchemes = "Asymmetric")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var username = (HttpContext.User.Identity as ClaimsIdentity)?.Name;
                if (username == null)
                    return BadRequest();
                var subject = await _subjectsRepository.GetByIdAsync(id, username);
                if (subject == null)
                    return NotFound();
                _subjectsRepository.Remove(subject);
                var success = await _unitOfWork.CompleteAsync();
                if (success > 0)
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
}
