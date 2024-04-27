using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;

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

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(NewUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            await _repository.Add(user);

            return CreatedAtAction(nameof(GetUser), new { user?.Id }, _mapper.Map<UserDto>(user));
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