using Microsoft.EntityFrameworkCore;
using Winx_Cinema.Data;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Repositories
{
    public class FilmResolutionRepository : IFilmResolutionRepository
    {
        private readonly AppDbContext _context;
        public FilmResolutionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<FilmResolution>> GetAll() => await _context.FilmResolutions.ToListAsync();

        public async Task<FilmResolution?> Get(Guid id) => await _context.FilmResolutions.FirstOrDefaultAsync(fr => fr.Id == id);

        public async Task<ICollection<FilmResolution>?> GetAllByFilmId(Guid filmId)
        {
            var film = await _context.Films.Where(f => f.Id == filmId)
                .Include(f => f.FilmResolutions).FirstOrDefaultAsync();

            return film?.FilmResolutions;
        }

        public async Task<bool> Add(Guid filmId, FilmResolution filmResolution)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == filmId);

            if (film == null)
                return false;

            filmResolution.Film = film;

            _context.FilmResolutions.Add(filmResolution);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(FilmResolution filmResolution)
        {
            _context.Entry(filmResolution).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(filmResolution.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var filmResolution = await Get(id);
            if (filmResolution == null)
                return false;

            _context.FilmResolutions.Remove(filmResolution);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(Guid id) => _context.FilmResolutions.Any(fr => fr.Id == id);
    }
}
