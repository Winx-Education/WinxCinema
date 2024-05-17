using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;
using Winx_Cinema.Shared.Enums;

namespace Winx_Cinema.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAll();
        Task<User?> Get(string id);
        Task<LoginRegisterResults> CreateUser(NewUserDto user);
        Task<LoginResponseDto> Login(LoginDto userDto);
        Task<bool> Update(User user);
        Task<bool> Delete(string id);
        bool Exists(string id);
    }
}
