using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;
using Winx_Cinema.Shared.Enums;

namespace Winx_Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<ICollection<UserDto>>> GetUsers() =>
            Ok(_mapper.Map<List<UserDto>>(await _repository.GetAll()));

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var user = await _repository.Get(id);

            if (user == null)
                return NotFound("User wasn't found");

            return Ok(_mapper.Map<UserDto>(user));
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, NewUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.Id = id;

            if (!await _repository.Update(user))
                return NotFound("User wasn't found");

            return NoContent();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(LoginDto model)
        {
            var result = await _repository.Login(model);

            switch (result.LoginResults)
            {
                case LoginRegisterResults.Ok:
                    return Ok(result);
                default:
                    return BadRequest(result);
            }
        }
        // POST: api/Users
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> PostUser(NewUserDto dto)
        {
            var result = await _repository.CreateUser(dto);

            switch (result)
            {
                case LoginRegisterResults.Ok:
                    return Ok(new RegisterResponseDto() { Result = LoginRegisterResults.Ok });
                case LoginRegisterResults.EmailIsExist:
                    return BadRequest(new RegisterResponseDto() { Result = LoginRegisterResults.EmailIsExist });
                case LoginRegisterResults.UserNameIsExist:
                    return BadRequest(new RegisterResponseDto() { Result = LoginRegisterResults.UserNameIsExist});
                default:
                    return BadRequest(new RegisterResponseDto(){Result = LoginRegisterResults.SomethingWentWrong});
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!await _repository.Delete(id))
                return NotFound("User wasn't found");

            return NoContent();
        }
    }
}