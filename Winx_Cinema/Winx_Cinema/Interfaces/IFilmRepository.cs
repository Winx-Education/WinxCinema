using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Interfaces
{
    public interface IFilmRepository
    {
        Task<PagedEntities<Film>> GetAll(string? search, string[] sortBy,
            string? genre, string? rating, string? date, int page, int pageLimit);
        Task<Film?> Get(Guid id);
        Task Add(Film film);
        Task<bool> Update(Film film);
        Task<bool> Delete(Guid id);
        bool Exists(Guid id);
    }
}
