using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.DTOs.AuthDtos
{
    public class UserLoginResponseDto
    {
        public string? Msg { get; set; }
        public string? Token { get; set; }
        public bool IsOk { get; set; }
        public string? ExpiresIn { get; set; }
    }
}
