namespace Winx_Cinema.Shared.Entities
{
    public class PagedEntities<T>
    {
        public int TotalPages { get; set; }
        public IEnumerable<T> Entities { get; set; } = null!;
    }
}
