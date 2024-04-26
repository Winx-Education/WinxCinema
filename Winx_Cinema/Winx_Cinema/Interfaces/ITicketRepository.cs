using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface ITicketRepository
    {
        Task<ICollection<Ticket>> GetTicketsAsync();
        Task<Ticket?> GetTicketAsync(Guid id);
        Task<ICollection<Ticket>?> GetTicketsBySessionIdAsync(Guid sessionId);
        Task<ICollection<Ticket>?> GetTicketsByUserIdAsync(string userId);
        Task AddTicketAsync(Ticket ticket);
        Task<bool> UpdateTicketAsync(Ticket ticket);
        Task<bool> DeleteTicketAsync(Guid id);
    }
}
