using System.Linq.Expressions;
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

        public async Task<ICollection<Film>> GetAll(string? search, string[] sortBy)
        {
            IQueryable<Film> films = _context.Films;

            // Sorting
            films = Search(films, search);

            // Sorting
            films = Sort(films, sortBy);

            return await films.ToListAsync();
        }

        private static IQueryable<Film> Search(IQueryable<Film> films, string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return films;

            search = search.ToLower().Trim();

            // Search for films with similar title, director or cast
            return films.Where(f => f.Title.ToLower().Contains(search)
                || f.Director.ToLower().Contains(search)
                || f.Cast.ToLower().Contains(search));
        }

        private static IQueryable<Film> Sort(IQueryable<Film> films, string[] sortBy)
        {
            IOrderedQueryable<Film> sortedFilms = null!;

            bool wasSorted = false;
            sortBy = sortBy.Distinct().ToArray(); // remove duplicates
            foreach (var sortField in sortBy)
            {
                if (string.IsNullOrWhiteSpace(sortField))
                    continue;

                string sortCriteria = sortField.Trim();

                // Get sorting order
                bool ascending = true;
                if (sortCriteria.StartsWith('-'))
                {
                    sortCriteria = sortCriteria.TrimStart('-');
                    ascending = false;
                }

                // Get sorting criteria
                Expression<Func<Film, object>>? sortKey;
                sortKey = sortCriteria switch
                {
                    "rating" => f => f.Rating,
                    "date" => f => f.ReleaseDate,
                    _ => null
                };

                // Ignore not registered criterias
                if (sortKey == null)
                    continue;

                // Check if was sorted
                if (!wasSorted)
                {
                    wasSorted = true;
                    // Apply primary sort
                    if (ascending)
                    {
                        sortedFilms = films.OrderBy(sortKey);
                        continue;
                    }
                    sortedFilms = films.OrderByDescending(sortKey);
                    continue;
                }

                // Apply secondary sort
                if (ascending)
                {
                    sortedFilms = sortedFilms.ThenBy(sortKey);
                    continue;
                }
                sortedFilms = sortedFilms.ThenByDescending(sortKey);
            }

            if (wasSorted)
            {
                return sortedFilms;
            }
            return films;
        }

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
