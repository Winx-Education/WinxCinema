﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Shared.Dtos
{
    public class LoginResponseDto
    {
        public User user { get; set; }
        public string Token { get; set; }
    }
}
