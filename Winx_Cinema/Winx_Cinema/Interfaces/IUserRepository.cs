using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAll();
        Task<User?> Get(string id);
        Task Add(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(string id);
        bool Exists(string id);
    }
}
