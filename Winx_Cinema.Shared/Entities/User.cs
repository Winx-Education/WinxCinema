using Microsoft.AspNetCore.Identity;

namespace Winx_Cinema.Shared.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
