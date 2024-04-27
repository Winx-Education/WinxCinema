namespace Winx_Cinema.Shared.Dtos
{
    public class FilmResolutionDto
    {
        public Guid Id { get; set; }
        public Guid FilmId { get; set; }
        public string Resolution { get; set; }
        public double Price { get; set; }
    }
}
