namespace Winx_Cinema.Shared.Dtos
{
    public class NewFilmDto
    {
        public string Title { get; set; }
        public string PicturePath { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string TrailerPath { get; set; }
        public TimeSpan Duration { get; set; }
        public int Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
