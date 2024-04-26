using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface IFilmRepository
    {
        Task<ICollection<Film>> GetFilmsAsync();
        Task<Film?> GetFilmAsync(Guid id);
        Task AddFilmAsync(Film film);
        Task<bool> UpdateFilmAsync(Film film);
        Task<bool> DeleteFilmAsync(Guid id);
    }
}
