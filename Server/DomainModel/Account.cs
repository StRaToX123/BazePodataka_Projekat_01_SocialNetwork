using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DomainModel
{
    public class Account
    {
        public String id { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String email { get; set; }
        public String isOnline { get; set; }
        public List<Account> friends { get; set; }
    }
}
