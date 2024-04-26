using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface IFilmResolutionRepository
    {
        Task<ICollection<FilmResolution>> GetAll();
        Task<FilmResolution?> Get(Guid id);
        Task<ICollection<FilmResolution>?> GetAllByFilmId(Guid filmId);
        Task<bool> Add(Guid filmId, FilmResolution filmResolution);
        Task<bool> Update(FilmResolution filmResolution);
        Task<bool> Delete(Guid id);
        bool Exists(Guid id);
    }
}
