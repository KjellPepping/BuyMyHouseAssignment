using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class User
    {
        public string name { get; set; }
        public string email { get; set; }
        public House house { get; set; }
        public double income { get; set; }
    }
}
