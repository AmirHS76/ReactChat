using ReactChat.Core.Entities.Login;
using ReactChat.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Core.Entities.User
{
    public class RegularUser : BaseUser
    {
        RegularAccesses RegularAccesses { get; set; }
        public RegularUser() 
        {
            Role = base.Role;
        }
    }
}
