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

        public async Task<ICollection<Ticket>> GetTicketsAsync() => await _context.Tickets.ToListAsync();

        public async Task<Ticket?> GetTicketAsync(Guid id) => await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

        public async Task<ICollection<Ticket>?> GetTicketsBySessionIdAsync(Guid sessionId)
        {
            var session = await _context.Sessions.Where(s => s.Id == sessionId)
                .Include(s => s.Tickets).FirstOrDefaultAsync();

            return session?.Tickets;
        }

        public async Task<ICollection<Ticket>?> GetTicketsByUserIdAsync(string userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId)
                .Include(u => u.Tickets).FirstOrDefaultAsync();

            return user?.Tickets;
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateTicketAsync(Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tickets.Any(t => t.Id == ticket.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> DeleteTicketAsync(Guid id)
        {
            var ticket = await GetTicketAsync(id);
            if (ticket == null)
                return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
