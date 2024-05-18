using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepo;
        private readonly IFilmResolutionRepository _filmResRepo;
        private readonly IHallRepository _hallRepo;
        private readonly IMapper _mapper;

        public SessionsController(ISessionRepository sessionRepo, IFilmResolutionRepository filmResRepo, IHallRepository hallRepo, IMapper mapper)
        {
            _sessionRepo = sessionRepo;
            _filmResRepo = filmResRepo;
            _hallRepo = hallRepo;
            _mapper = mapper;
        }

        // GET: api/Sessions
        [HttpGet]
        public async Task<ActionResult<ICollection<SessionDto>>> GetSessions([FromQuery] string? time) =>
            Ok(_mapper.Map<List<SessionDto>>(await _sessionRepo.GetAll(time)));

        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionDto>> GetSession(Guid id)
        {
            var session = await _sessionRepo.Get(id);

            if (session == null)
                return NotFound("Session wasn't found");

            return Ok(_mapper.Map<SessionDto>(session));
        }

        // GET: api/Sessions/FilmResolutions/5
        [HttpGet("FilmResolutions/{filmResId}")]
        public async Task<ActionResult<SessionDto>> GetSessionsByFilmResolutionId(Guid filmResId)
        {
            var sessions = await _sessionRepo.GetAllByFilmResolutionId(filmResId);

            if (sessions == null)
                return NotFound("FilmResolution wasn't found");

            return Ok(_mapper.Map<List<SessionDto>>(sessions));
        }

        // PUT: api/Sessions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSession(Guid id, NewSessionDto dto)
        {
            var session = _mapper.Map<Session>(dto);
            session.Id = id;

            if (!await _sessionRepo.Update(session))
                return NotFound("Session wasn't found");

            return NoContent();
        }

        // POST: api/Sessions/FilmResolutions/
        [HttpPost]
        public async Task<ActionResult<SessionDto>> PostSession([FromQuery]Guid filmResId, [FromQuery]Guid hallId, NewSessionDto dto)
        {
            if (!_filmResRepo.Exists(filmResId))
                return NotFound("FilmResolution wasn't found");

            if (!_hallRepo.Exists(hallId))
                return NotFound("Hall wasn't found");

            var session = _mapper.Map<Session>(dto);
            await _sessionRepo.Add(filmResId, hallId, session);

            return CreatedAtAction(nameof(GetSession), new { session?.Id }, _mapper.Map<SessionDto>(session));
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            if (!await _sessionRepo.Delete(id))
                return NotFound("Session wasn't found");

            return NoContent();
        }
    }
}