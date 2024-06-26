using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<PagedEntities<HallDto>>> GetHalls(
            [FromQuery] int page = 1, [FromQuery] int pageLimit = 10)
        {
            var result = await _repository.GetAll(page, pageLimit);
            var dtos = _mapper.Map<List<HallDto>>(result.Entities);

            return Ok(new PagedEntities<HallDto>
            {
                TotalPages = result.TotalPages,
                Entities = dtos
            });
        }
          

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
        [Authorize(Roles = "adminUser")]
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
        [Authorize(Roles = "adminUser")]
        public async Task<ActionResult<HallDto>> PostHall(NewHallDto dto)
        {
            var hall = _mapper.Map<Hall>(dto);
            await _repository.Add(hall);

            return CreatedAtAction(nameof(GetHall), new { hall?.Id }, _mapper.Map<HallDto>(hall));
        }

        // DELETE: api/Halls/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "adminUser")]
        public async Task<IActionResult> DeleteHall(Guid id)
        {
            if (!await _repository.Delete(id))
                return NotFound("Hall wasn't found");

            return NoContent();
        }
    }
}