using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.DTOs.AuthDtos
{
    public class UserSignupRequestDto
    {
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
