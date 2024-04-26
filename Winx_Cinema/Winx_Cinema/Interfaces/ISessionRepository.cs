using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface ISessionRepository
    {
        Task<ICollection<Session>> GetSessionsAsync();
        Task<Session?> GetSessionAsync(Guid id);
        Task<ICollection<Session>?> GetSessionsByFilmResolutionIdAsync(Guid filmResId);
        Task AddSessionAsync(Session session);
        Task<bool> UpdateSessionAsync(Session session);
        Task<bool> DeleteSessionAsync(Guid id);
    }
}
