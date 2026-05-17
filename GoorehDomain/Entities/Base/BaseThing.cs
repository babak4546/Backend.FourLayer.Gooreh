using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities.Base
{
    public class BaseThing
    {
        //thing baraye karbar ha bood baraye qir karbar h gable estefadeh nist
        public int Id { get; set; }
        public string? Guid { get; set; } 
        public DateTime CreatedIn { get; set; }
        public DateTime EditedIn { get; set; }

    }
}
