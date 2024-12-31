using ReactChat.Core.Enums;

namespace ReactChat.Core.Entities.User
{
    public class RegularUser : BaseUser
    {
#pragma warning disable IDE0051 // Remove unused private members
        RegularAccesses RegularAccesses { get; set; }
        public RegularUser()
        {
            Role = UserRole.RegularUser;
        }
    }
}
