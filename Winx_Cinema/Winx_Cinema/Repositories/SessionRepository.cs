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

        public async Task<ICollection<Session>> GetAll() => await _context.Sessions.ToListAsync();

        public async Task<Session?> Get(Guid id) => await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);

        public async Task<ICollection<Session>?> GetAllByFilmResolutionId(Guid filmResId)
        {
            var filmResolution = await _context.FilmResolutions.Where(fr => fr.Id == filmResId)
                .Include(fr => fr.Sessions).FirstOrDefaultAsync();

            return filmResolution?.Sessions;
        }

        public async Task<bool> Add(Guid filmResId, Guid hallId, Session session)
        {
            var filmResolution = await _context.FilmResolutions.FirstOrDefaultAsync(f => f.Id == filmResId);

            if (filmResolution == null)
                return false;

            var hall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == hallId);

            if (hall == null)
                return false;

            session.FilmResolution = filmResolution;
            session.Hall = hall;

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Session session)
        {
            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(session.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var session = await Get(id);
            if (session == null)
                return false;

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(Guid id) => _context.Sessions.Any(s => s.Id == id);
    }
}