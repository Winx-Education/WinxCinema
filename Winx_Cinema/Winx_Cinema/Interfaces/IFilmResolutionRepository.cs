using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface IFilmResolutionRepository
    {
        Task<ICollection<FilmResolution>> GetFilmResolutionsAsync();
        Task<FilmResolution?> GetFilmResolutionAsync(Guid id);
        Task<ICollection<FilmResolution>?> GetFilmResolutionsByFilmIdAsync(Guid filmId);
        Task AddFilmResolutionAsync(FilmResolution filmResolution);
        Task<bool> UpdateFilmResolutionAsync(FilmResolution filmResolution);
        Task<bool> DeleteFilmResolutionAsync(Guid id);
    }
}
