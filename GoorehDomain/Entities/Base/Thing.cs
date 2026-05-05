using GoorehDomain.Interfaces;

namespace GoorehDomain.Entities.Base
{
    public class Thing : IThing
    {
        public  int Id { get; set; }
        public  string Guid { get; set; } =System.Guid.NewGuid().ToString();
        public  DateTime CreatedIn { get; set; }
        public  DateTime EditedIn { get; set; }
        public String? UpperUsername { get; set; }
    }
}