using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DomainModel
{
    class ChatEntry
    {
        public String id { get; set; } // redni broj u sekvenci poruka
        public String date { get; set; } // kada je ova poruka poslata
        public String sender { get; set; } // username onoga koji je poslao
        public String text { get; set; } // sadrzaj poruke
    }
}
