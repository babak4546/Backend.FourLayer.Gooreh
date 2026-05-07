using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.DTOs.LogDtos
{
    public class SpecialUserLogData
    {
        public DateTime? LogDate { get; set; }
        public DateTime? LogedIn { get; set; }
        public DateTime? LoggedOut { get; set; }
        public required string Action { get; set; }
        public string? IpAddr { get; set; } = "IpIsEmpty";
        public string? SysInfo { get; set; } = "SysInfoIsNull";
    }
}
