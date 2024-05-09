namespace Winx_Cinema.Shared.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int SitNumber { get; set; }

        public Session Session { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
