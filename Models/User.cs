using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class User : TableEntity
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public House house { get; set; }
        public double income { get; set; }
        public string city { get; set; }
    }
}
