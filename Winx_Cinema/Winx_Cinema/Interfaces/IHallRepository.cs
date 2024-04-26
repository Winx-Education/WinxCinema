using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface IHallRepository
    {
        Task<ICollection<Hall>> GetHallsAsync();
        Task<Hall?> GetHallAsync(Guid id);
        Task AddHallAsync(Hall hall);
        Task<bool> UpdateHallAsync(Hall hall);
        Task<bool> DeleteHallAsync(Guid id);
    }
}
