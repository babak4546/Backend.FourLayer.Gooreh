namespace GoorehDomain.Interfaces
{
    public interface IVirtualRemove
    {

        DateTime? RemovedIn { get; set; }
        bool IsRemoved { get; set; }
        
    }
}
