using GoorehDomain.Entities.Base;
using GoorehDomain.Enums;
using GoorehDomain.Interfaces;

namespace GoorehDomain.Entities
{
    public class AppUser : Thing, IAppUser, IVirtualRemove
    {

        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; }
        public string? NormalizedUsername { get; set; }
        public string? PasswordHash { get; set; }
        public UserTypeEnum UserType { get; set; } = UserTypeEnum.BoxUser;
        public DateTime? RemovedIn { get; set; }
        public DateTime? RestoredIn { get; set; }
        public bool IsRemoved { get; set; }
        public string? Salt { get; set; }
        public int AccessFailedCount { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; } = true;
        public string UserTypes
        {
            get
            {
                switch (this.UserType)
                {
                    case UserTypeEnum.AppAdmin:
                        return "ادمین برنامه";
                    case UserTypeEnum.BoxOwner:
                        return "صاحب صندوق";
                        case UserTypeEnum.BoxAdmin:
                        return "ادمین صندوق";
                    case UserTypeEnum.BoxUser:
                        return "کاربر صندوق";

                    default:
                        break;
                }
                return "ناشناخته";
            }  
        }
    }
}
