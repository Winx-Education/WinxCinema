using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmRepository _repository;
        private readonly IMapper _mapper;

        public FilmsController(IFilmRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Films
        [HttpGet]
        public async Task<ActionResult<PagedEntities<FilmDto>>> GetFilms([FromQuery] string? search,
            [FromQuery] string[] sortBy, [FromQuery] string? genre,
            [FromQuery] string? rating, [FromQuery] string? date,
            [FromQuery] int page = 1, [FromQuery] int pageLimit = 10)
        {
            var result = await _repository.GetAll(search, sortBy, genre, rating, date, page, pageLimit);
            var dtos = _mapper.Map<List<FilmDto>>(result.Entities);
            return Ok(new PagedEntities<FilmDto>()
            {
                TotalPages = result.TotalPages,
                Entities = dtos
            });
        }

        // GET: api/Films/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmDto>> GetFilm(Guid id)
        {
            var film = await _repository.Get(id);

            if (film == null)
                return NotFound("Film wasn't found");

            return Ok(_mapper.Map<FilmDto>(film));
        }

        // PUT: api/Films/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(Guid id, NewFilmDto dto)
        {
            var film = _mapper.Map<Film>(dto);
            film.ReleaseDate = DateTime.SpecifyKind(film.ReleaseDate, DateTimeKind.Utc);
            film.Id = id;

            if (!await _repository.Update(film))
                return NotFound("Film wasn't found");

            return NoContent();
        }

        // POST: api/Films
        [HttpPost]
        public async Task<ActionResult<FilmDto>> PostFilm(NewFilmDto dto)
        {
            var film = _mapper.Map<Film>(dto);
            await _repository.Add(film);

            return CreatedAtAction(nameof(GetFilm), new { film?.Id }, _mapper.Map<FilmDto>(film));
        }

        // DELETE: api/Films/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(Guid id)
        {
            if (!await _repository.Delete(id))
                return NotFound("Film wasn't found");

            return NoContent();
        }
    }
}