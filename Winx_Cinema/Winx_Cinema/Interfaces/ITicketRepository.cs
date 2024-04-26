using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface ITicketRepository
    {
        Task<ICollection<Ticket>> GetAll();
        Task<Ticket?> Get(Guid id);
        Task<ICollection<Ticket>?> GetAllBySessionId(Guid sessionId);
        Task<ICollection<Ticket>?> GetAllByUserId(string userId);
        Task<bool> Add(Guid sessionId, string userId, Ticket ticket);
        Task<bool> Update(Ticket ticket);
        Task<bool> Delete(Guid id);
        bool Exists(Guid id);
    }
}
