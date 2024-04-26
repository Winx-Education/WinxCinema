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

        public async Task<ICollection<FilmResolution>> GetFilmResolutionsAsync() => await _context.FilmResolutions.ToListAsync();

        public async Task<FilmResolution?> GetFilmResolutionAsync(Guid id) => await _context.FilmResolutions.FirstOrDefaultAsync(fr => fr.Id == id);

        public async Task<ICollection<FilmResolution>?> GetFilmResolutionsByFilmIdAsync(Guid filmId)
        {
            var film = await _context.Films.Where(f => f.Id == filmId)
                .Include(f => f.FilmResolutions).FirstOrDefaultAsync();

            return film?.FilmResolutions;
        }

        public async Task AddFilmResolutionAsync(FilmResolution filmResolution)
        {
            _context.FilmResolutions.Add(filmResolution);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateFilmResolutionAsync(FilmResolution filmResolution)
        {
            _context.Entry(filmResolution).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FilmResolutions.Any(fr => fr.Id == filmResolution.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> DeleteFilmResolutionAsync(Guid id)
        {
            var filmResolution = await GetFilmResolutionAsync(id);
            if (filmResolution == null)
                return false;

            _context.FilmResolutions.Remove(filmResolution);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
