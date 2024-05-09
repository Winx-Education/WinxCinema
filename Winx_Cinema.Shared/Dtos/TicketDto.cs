namespace Winx_Cinema.Shared.Dtos
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int SitNumber { get; set; }
    }
}
