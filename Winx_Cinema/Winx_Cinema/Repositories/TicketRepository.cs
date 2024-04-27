using Microsoft.EntityFrameworkCore;
using Winx_Cinema.Data;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;
        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Ticket>> GetAll() => await _context.Tickets.ToListAsync();

        public async Task<Ticket?> Get(Guid id) => await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

        public async Task<ICollection<Ticket>?> GetAllBySessionId(Guid sessionId)
        {
            var session = await _context.Sessions.Where(s => s.Id == sessionId)
                .Include(s => s.Tickets).FirstOrDefaultAsync();

            return session?.Tickets;
        }

        public async Task<ICollection<Ticket>?> GetAllByUserId(string userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId)
                .Include(u => u.Tickets).FirstOrDefaultAsync();

            return user?.Tickets;
        }

        public async Task<bool> Add(Guid sessionId, string userId, Ticket ticket)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
                return false;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            ticket.Session = session;
            ticket.User = user;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Ticket ticket)
        {
            try
            {
                var oldTicket = await _context.Tickets.FirstAsync(t => t.Id == ticket.Id);
                _context.Entry(oldTicket).State = EntityState.Detached;
                ticket.SessionId = oldTicket.SessionId;
                ticket.UserId = oldTicket.UserId;

                _context.Entry(ticket).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException || ex is InvalidOperationException)
            {
                if (!Exists(ticket.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var ticket = await Get(id);
            if (ticket == null)
                return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(Guid id) => _context.Tickets.Any(t => t.Id == id);
    }
}
