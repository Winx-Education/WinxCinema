namespace Winx_Cinema.Shared.Dtos
{
    public class SessionDto
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Guid FilmResolutionId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
