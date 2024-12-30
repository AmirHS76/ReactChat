using ReactChat.Core.Enums;

namespace ReactChat.Core.Entities.User
{
    public class AdminUser : BaseUser
    {
        AdminAccesses AdminAccesses { get; set; }
        public AdminUser()
        {
            Role = base.Role;
        }
    }
}
