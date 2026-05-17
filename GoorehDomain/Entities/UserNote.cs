using GoorehDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities
{
    public class UserNote:BaseThing
    {
        public AppUser? AppUser { get; set; }
        public int AppUserId { get; set; }
        public string? Title { get; set; }
        public string?  Text { get; set; }
    }
}
