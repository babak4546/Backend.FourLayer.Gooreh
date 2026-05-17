using GoorehDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities
{
    public class UserContact:BaseThing
    {
        public AppUser? AppUser { get; set; }
        public int? AppUserId { get; set; }
        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
