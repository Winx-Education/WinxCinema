using Microsoft.AspNetCore.Mvc;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface ISessionRepository
    {
        Task<ICollection<Session>> GetAll(string? time);
        Task<Session?> Get(Guid id);
        Task<ICollection<Session>?> GetAllByFilmResolutionId(Guid filmResId, string? time);
        Task<bool> Add(Guid filmResId, Guid hallId, Session session);
        Task<bool> Update(Session session);
        Task<bool> Delete(Guid id);
        bool Exists(Guid id);
    }
}
