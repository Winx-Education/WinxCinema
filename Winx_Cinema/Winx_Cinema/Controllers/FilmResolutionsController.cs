using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmResolutionsController : ControllerBase
    {
        private readonly IFilmResolutionRepository _filmResRepo;
        private readonly IFilmRepository _filmRepo;
        private readonly IMapper _mapper;

        public FilmResolutionsController(IFilmResolutionRepository filmResRepo, IFilmRepository filmRepo, IMapper mapper)
        {
            _filmResRepo = filmResRepo;
            _filmRepo = filmRepo;
            _mapper = mapper;
        }

        // GET: api/FilmResolutions
        [HttpGet]
        public async Task<ActionResult<ICollection<FilmResolutionDto>>> GetFilmResolutions() =>
            Ok(_mapper.Map<List<FilmResolutionDto>>(await _filmResRepo.GetAll()));

        // GET: api/FilmResolutions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmResolutionDto>> GetFilmResolution(Guid id)
        {
            var filmResolution = await _filmResRepo.Get(id);

            if (filmResolution == null)
                return NotFound("FilmResolution wasn't found");

            return Ok(_mapper.Map<FilmResolutionDto>(filmResolution));
        }

        // GET: api/FilmResolutions/Films/5
        [HttpGet("Films/{filmId}")]
        public async Task<ActionResult<FilmResolutionDto>> GetFilmResolutionsByFilmId(Guid filmId)
        {
            var filmResolutions = await _filmResRepo.GetAllByFilmId(filmId);

            if (filmResolutions == null)
                return NotFound("Film wasn't found");

            return Ok(_mapper.Map<List<FilmResolutionDto>>(filmResolutions));
        }

        // PUT: api/FilmResolutions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilmResolution(Guid id, NewFilmResolutionDto dto)
        {
            var filmResolution = _mapper.Map<FilmResolution>(dto);
            filmResolution.Id = id;

            if (!await _filmResRepo.Update(filmResolution))
                return NotFound("FilmResolution wasn't found");

            return NoContent();
        }

        // POST: api/FilmResolutions/Films/5
        [HttpPost]
        public async Task<ActionResult<FilmResolutionDto>> PostFilmResolution([FromQuery]Guid filmId, NewFilmResolutionDto dto)
        {
            if (!_filmRepo.Exists(filmId))
                return NotFound("Film wasn't found");

            var filmResolution = _mapper.Map<FilmResolution>(dto);
            await _filmResRepo.Add(filmId, filmResolution);

            return CreatedAtAction(nameof(GetFilmResolution), new { filmResolution?.Id }, _mapper.Map<FilmResolutionDto>(filmResolution));
        }

        // DELETE: api/FilmResolutions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilmResolution(Guid id)
        {
            if (!await _filmResRepo.Delete(id))
                return NotFound("FilmResolution wasn't found");

            return NoContent();
        }
    }
}