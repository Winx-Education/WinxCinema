namespace Winx_Cinema.Shared.Entities
{
    public class Film
    {
        public Guid Id { get; set; }
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

        public ICollection<FilmResolution> FilmResolutions { get; }
    }
}
