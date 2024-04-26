using Microsoft.EntityFrameworkCore;
using Winx_Cinema.Data;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly AppDbContext _context;
        public FilmRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Film>> GetAll() => await _context.Films.ToListAsync();

        public async Task<Film?> Get(Guid id) => await _context.Films.FirstOrDefaultAsync(f => f.Id == id);

        public async Task Add(Film film)
        {
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Film film)
        {
            _context.Entry(film).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(film.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var film = await Get(id);
            if (film == null)
                return false;

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(Guid id) => _context.Films.Any(f => f.Id == id);
    }
}
