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

        public async Task<ICollection<Hall>> GetAll() => await _context.Halls.ToListAsync();

        public async Task<Hall?> Get(Guid id) => await _context.Halls.FirstOrDefaultAsync(h => h.Id == id);

        public async Task Add(Hall hall)
        {
            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Hall hall)
        {
            _context.Entry(hall).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(hall.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var hall = await Get(id);
            if (hall == null)
                return false;

            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(Guid id) => _context.Halls.Any(h => h.Id == id);
    }
}
