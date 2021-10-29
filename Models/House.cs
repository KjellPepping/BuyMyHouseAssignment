using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;

namespace Models
{
    public class House : TableEntity
    {
        public int id { get; set; }

        public double price { get; set; }

        public string imageUrl { get; set; }

        public string streetName { get; set;}

        public int streetNumber { get; set; }

        public string postalCode { get; set; }

        public string city { get; set; }
    }
}
