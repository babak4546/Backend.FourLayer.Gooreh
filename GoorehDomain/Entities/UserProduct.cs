using GoorehDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities
{
    public class UserProduct :BaseThing
    {
        public string? Title { get; set; }
        public string? Value { get; set; }
        public  string? ConcurrencyStamp { get; set; }
    }
}
