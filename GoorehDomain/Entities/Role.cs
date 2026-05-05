using GoorehDomain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public required UserTypeEnum RoleTitle { get; set; } 
    }
}
