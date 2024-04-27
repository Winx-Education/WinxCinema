using Microsoft.EntityFrameworkCore;
using Winx_Cinema.Data;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<User>> GetAll() => await _context.Users.ToListAsync();

        public async Task<User?> Get(string id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(user.Id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var user = await Get(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(string id) => _context.Users.Any(u => u.Id == id);
    }
}
