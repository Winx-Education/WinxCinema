using Microsoft.AspNetCore.Identity;

namespace Winx_Cinema.Shared.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;

        public ICollection<Ticket> Tickets { get; } = null!;
    }
}
