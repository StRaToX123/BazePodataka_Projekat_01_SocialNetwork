using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.DomainModel
{
    public class Movie
    {
        public String id { get; set; }
        public String title { get; set; }
        public int year { get; set; }
        public List<Role> cast { get; set; }
    }
}
