using Microsoft.EntityFrameworkCore;
using Winx_Cinema.Data;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly AppDbContext _context;
        public HallRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Hall>> GetHallsAsync() => await _context.Halls.ToListAsync();

        public async Task<Hall?> GetHallAsync(Guid id) => await _context.Halls.FirstOrDefaultAsync(h => h.Id == id);

        public async Task AddHallAsync(Hall hall)
        {
            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateHallAsync(Hall hall)
        {
            _context.Entry(hall).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HallExists(hall.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> DeleteHallAsync(Guid id)
        {
            var hall = await GetHallAsync(id);
            if (hall == null)
                return false;

            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool HallExists(Guid id) => _context.Halls.Any(h => h.Id == id);
    }
}
