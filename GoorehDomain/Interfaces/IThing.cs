namespace GoorehDomain.Interfaces
{
    public interface IThing
    {
         int Id { get; set; }
         string Guid { get; set; }
        DateTime CreatedIn { get; set; }
        DateTime EditedIn { get; set; }
    }
}
