using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoorehDomain.Entities
{
    public class UserRole
    {
        
        public  int Id { get; set; }
        public AppUser? AppUser { get; set; }
        public int  AppUserId  { get; set; }
        public Role? Role { get; set; }
        public int RoleId { get; set; }
    }
}
