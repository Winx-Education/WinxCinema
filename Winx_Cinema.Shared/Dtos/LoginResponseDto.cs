using Winx_Cinema.Shared.Enums;

namespace Winx_Cinema.Shared.Dtos
{
    public class LoginResponseDto
    {
        public UserDto user { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public LoginRegisterResults LoginResults { get; set; }
    }
}
