using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly IHallRepository _repository;
        private readonly IMapper _mapper;

        public HallsController(IHallRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Halls
        [HttpGet]
        public async Task<ActionResult<ICollection<HallDto>>> GetHalls() =>
            Ok(_mapper.Map<List<HallDto>>(await _repository.GetAll()));

        // GET: api/Halls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HallDto>> GetHall(Guid id)
        {
            var hall = await _repository.Get(id);

            if (hall == null)
                return NotFound("Hall wasn't found");

            return Ok(_mapper.Map<HallDto>(hall));
        }

        // PUT: api/Halls/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHall(Guid id, NewHallDto dto)
        {
            var hall = _mapper.Map<Hall>(dto);
            hall.Id = id;

            if (!await _repository.Update(hall))
                return NotFound("Hall wasn't found");

            return NoContent();
        }

        // POST: api/Halls
        [HttpPost]
        public async Task<ActionResult<HallDto>> PostHall(NewHallDto dto)
        {
            var hall = _mapper.Map<Hall>(dto);
            await _repository.Add(hall);

            return CreatedAtAction(nameof(GetHall), new { hall?.Id }, _mapper.Map<HallDto>(hall));
        }

        // DELETE: api/Halls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHall(Guid id)
        {
            if (!await _repository.Delete(id))
                return NotFound("Hall wasn't found");

            return NoContent();
        }
    }
}