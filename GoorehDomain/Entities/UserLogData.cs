using GoorehDomain.Entities.Base;
using GoorehDomain.Interfaces;

namespace GoorehDomain.Entities
{
    public class UserLogData : LogThing,IUserLogData
    {
        public AppUser? AppUser { get; set; }
        public int AppUserId { get; set; }
        public DateTime? LogedIn { get; set; }
        public DateTime? LoggedOut { get; set; }
        public required string Action { get; set; }
        public string? IpAddr { get; set; } = "IpIsEmpty";
        public string? SysInfo { get; set; } = "SysInfoIsNull";
    }
}
