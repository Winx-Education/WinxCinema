namespace Winx_Cinema.Shared.Dtos
{
    public class HallDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }
        public string Resolution { get; set; } = string.Empty;
    }
}
