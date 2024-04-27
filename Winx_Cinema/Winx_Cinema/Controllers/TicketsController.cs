using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly ISessionRepository _sessionRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public TicketsController(ITicketRepository ticketRepo, ISessionRepository sessionRepo, IUserRepository userRepo, IMapper mapper)
        {
            _ticketRepo = ticketRepo;
            _sessionRepo = sessionRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<ICollection<TicketDto>>> GetTickets() =>
            Ok(_mapper.Map<List<TicketDto>>(await _ticketRepo.GetAll()));

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(Guid id)
        {
            var ticket = await _ticketRepo.Get(id);

            if (ticket == null)
                return NotFound("Ticket wasn't found");

            return Ok(_mapper.Map<TicketDto>(ticket));
        }

        // GET: api/Tickets/Sessions/5
        [HttpGet("Sessions/{sessionId}")]
        public async Task<ActionResult<TicketDto>> GetTicketsBySessionId(Guid sessionId)
        {
            var tickets = await _ticketRepo.GetAllBySessionId(sessionId);

            if (tickets == null)
                return NotFound("Session wasn't found");

            return Ok(_mapper.Map<List<TicketDto>>(tickets));
        }

        // GET: api/Tickets/Users/5
        [HttpGet("Users/{userId}")]
        public async Task<ActionResult<TicketDto>> GetTicketsByUserId(string userId)
        {
            var tickets = await _ticketRepo.GetAllByUserId(userId);

            if (tickets == null)
                return NotFound("User wasn't found");

            return Ok(_mapper.Map<List<TicketDto>>(tickets));
        }

        // PUT: api/Tickets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(Guid id, NewTicketDto dto)
        {
            var ticket = _mapper.Map<Ticket>(dto);
            ticket.Id = id;

            if (!await _ticketRepo.Update(ticket))
                return NotFound("Ticket wasn't found");

            return NoContent();
        }

        // POST: api/Tickets/Sessions/
        [HttpPost]
        public async Task<ActionResult<TicketDto>> PostTicket([FromQuery] Guid sessionId, [FromQuery] string userId, NewTicketDto dto)
        {
            if (!_sessionRepo.Exists(sessionId))
                return NotFound("Session wasn't found");

            if (!_userRepo.Exists(userId))
                return NotFound("User wasn't found");

            var ticket = _mapper.Map<Ticket>(dto);
            await _ticketRepo.Add(sessionId, userId, ticket);

            return CreatedAtAction(nameof(GetTicket), new { ticket?.Id }, _mapper.Map<TicketDto>(ticket));
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            if (!await _ticketRepo.Delete(id))
                return NotFound("Ticket wasn't found");

            return NoContent();
        }
    }
}