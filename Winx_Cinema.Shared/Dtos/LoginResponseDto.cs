using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winx_Cinema.Shared.Entities;
using Winx_Cinema.Shared.Enums;

namespace Winx_Cinema.Shared.Dtos
{
    public class LoginResponseDto
    {
        public UserDto user { get; set; }
        public string Token { get; set; }
        public LoginRegisterResults LoginResults { get; set; }
    }
}
