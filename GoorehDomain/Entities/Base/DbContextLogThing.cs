using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities.Base
{
    public class DbContextLogThing
    {
        public int Id { get; set; }
        public string? Guid { get; set; }
        public string? DoByUsername { get; set; }
        public DateTime CreatedIn { get; set; }
        public DateTime EditedIn { get; set; }
    }
}
