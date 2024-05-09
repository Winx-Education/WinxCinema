namespace Winx_Cinema.Shared.Dtos
{
    public class NewFilmDto
    {
        public string Title { get; set; } = string.Empty;
        public string PicturePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Cast { get; set; } = string.Empty;
        public string TrailerPath { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public int Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
