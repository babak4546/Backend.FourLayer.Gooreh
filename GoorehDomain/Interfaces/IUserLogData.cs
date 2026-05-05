

namespace GoorehDomain.Interfaces

{
    public interface IUserLogData
    {

       string? IpAddr { get; set; }
        string? SysInfo { get; set; }
        DateTime? LogedIn { get; set; }
        DateTime? LoggedOut { get; set; }

    }
}
