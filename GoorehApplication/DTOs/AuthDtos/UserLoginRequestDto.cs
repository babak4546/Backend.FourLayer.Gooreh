using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.DTOs.AuthDtos
{
    public class UserLoginRequestDto
    {
        public required  string UserName { get; set; }
        public required string Password { get; set; }
    }
}
