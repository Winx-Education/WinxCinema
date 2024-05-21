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
    public class FilmsController : ControllerBase
    {
        private readonly IFilmRepository _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FilmsController(IFilmRepository repository, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _repository = repository;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
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
        [Authorize(Roles = "adminUser")]
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
        [Authorize(Roles = "adminUser")]
        public async Task<ActionResult<FilmDto>> PostFilm(NewFilmDto dto)
        {
            var film = _mapper.Map<Film>(dto);
            await _repository.Add(film);

            return CreatedAtAction(nameof(GetFilm), new { film?.Id }, _mapper.Map<FilmDto>(film));
        }

        [HttpPost]
        [Route("upload")]
        [Authorize(Roles = "adminUser")]
        public async Task<IActionResult> uploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected");

            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Ok("File uploaded successfully");
        }

        // DELETE: api/Films/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "adminUser")]
        public async Task<IActionResult> DeleteFilm(Guid id)
        {
            if (!await _repository.Delete(id))
                return NotFound("Film wasn't found");

            return NoContent();
        }
    }
}