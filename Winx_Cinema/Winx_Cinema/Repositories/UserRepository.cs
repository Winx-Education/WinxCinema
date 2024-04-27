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
            try
            {
                var oldUser = await _context.Users.FirstAsync(u => u.Id == user.Id);
                _context.Entry(oldUser).State = EntityState.Detached;
                user.AccessFailedCount = oldUser.AccessFailedCount;
                user.ConcurrencyStamp = oldUser.ConcurrencyStamp;
                user.EmailConfirmed = oldUser.EmailConfirmed;
                user.LockoutEnabled = oldUser.LockoutEnabled;
                user.LockoutEnd = oldUser.LockoutEnd;
                user.NormalizedEmail = oldUser.NormalizedEmail;
                user.NormalizedUserName = oldUser.NormalizedUserName;
                user.PasswordHash = oldUser.PasswordHash;
                user.PhoneNumberConfirmed = oldUser.PhoneNumberConfirmed;
                user.SecurityStamp = oldUser.SecurityStamp;
                user.TwoFactorEnabled = oldUser.TwoFactorEnabled;
                user.UserName = oldUser.UserName;

                _context.Entry(user).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException || ex is InvalidOperationException)
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
