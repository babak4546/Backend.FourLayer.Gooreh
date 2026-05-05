namespace GoorehDomain.Interfaces
{
    public interface IAppUser
    {
        string Firstname { get; set; }
        string Lastname { get; set; }
        string PhoneNumber { get; set; }
        string Username { get; set; }
        string NormalizedUsername { get; set; }
        string PasswordHash { get; set; }

    }
}
