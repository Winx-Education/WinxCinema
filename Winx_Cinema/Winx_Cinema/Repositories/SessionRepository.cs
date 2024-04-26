using Microsoft.EntityFrameworkCore;
using Winx_Cinema.Data;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly AppDbContext _context;
        public SessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Session>> GetSessionsAsync() => await _context.Sessions.ToListAsync();

        public async Task<Session?> GetSessionAsync(Guid id) => await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);

        public async Task<ICollection<Session>?> GetSessionsByFilmResolutionIdAsync(Guid filmResId)
        {
            var filmResolution = await _context.FilmResolutions.Where(fr => fr.Id == filmResId)
                .Include(fr => fr.Sessions).FirstOrDefaultAsync();

            return filmResolution?.Sessions;
        }

        public async Task AddSessionAsync(Session session)
        {
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateSessionAsync(Session session)
        {
            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Sessions.Any(s => s.Id == session.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> DeleteSessionAsync(Guid id)
        {
            var session = await GetSessionAsync(id);
            if (session == null)
                return false;

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
