using ReactChat.Core.Entities.Login;
using ReactChat.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
