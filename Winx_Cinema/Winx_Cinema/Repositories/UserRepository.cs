using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Winx_Cinema.Data;
using Winx_Cinema.Interfaces;
using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;
using Winx_Cinema.Shared.Enums;

namespace Winx_Cinema.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        public UserRepository(AppDbContext context, IConfiguration configuration, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<LoginResponseDto> Login(LoginDto userDto)
        {
            var user = await userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                return new LoginResponseDto
                {
                    LoginResults = LoginRegisterResults.IncorrectEmail,
                };
            }

            var result = await userManager.CheckPasswordAsync(user, userDto.Password);

            if (!result)
            {
                return new LoginResponseDto
                {
                    LoginResults = LoginRegisterResults.IncorrectPassword,
                };
            }

            var token = GenerateJwtToken(user);

            var newUser = mapper.Map<UserDto>(user);

            return new LoginResponseDto
            {
                user = newUser,
                Token = token,
                LoginResults = LoginRegisterResults.Ok,
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyStr = _configuration["Jwt:Key"] ?? throw new ConfigurationMissingException();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(Array.Empty<Claim>()),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512),
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<ICollection<User>> GetAll() => await _context.Users.ToListAsync();

        public async Task<User?> Get(string id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<LoginRegisterResults> CreateUser(NewUserDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                return LoginRegisterResults.EmailIsExist;
            }

            user = await userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                return LoginRegisterResults.UserNameIsExist;
            }

            user = new User()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                UserName = model.UserName,
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return LoginRegisterResults.Ok;
            }
            return LoginRegisterResults.SomethingWentWrong;
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
