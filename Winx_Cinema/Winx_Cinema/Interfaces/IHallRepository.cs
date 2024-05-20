using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface IHallRepository
    {
        Task<PagedEntities<Hall>> GetAll(int page, int pageLimit);
        Task<Hall?> Get(Guid id);
        Task Add(Hall hall);
        Task<bool> Update(Hall hall);
        Task<bool> Delete(Guid id);
        bool Exists(Guid id);
    }
}
