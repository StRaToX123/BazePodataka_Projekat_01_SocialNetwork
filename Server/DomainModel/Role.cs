﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.DomainModel
{
    public class Role
    {
        public Movie movie { get; set; }
        public Actor actor { get; set; }
        public String role { get; set; }
    }
}
