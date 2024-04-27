namespace Winx_Cinema.Shared.Entities
{
    public class FilmResolution
    {
        public Guid Id { get; set; }
        public Guid FilmId { get; set; }
        public string Resolution { get; set; }
        public double Price { get; set; }

        public Film Film { get; set; }
        public ICollection<Session> Sessions { get; }
    }
}
