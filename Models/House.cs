using Newtonsoft.Json;
using System;

namespace Models
{
    public class House
    {
        public int id { get; set; }

        public double price { get; set; }

        public string imageUrl { get; set; }

        public string streetName { get; set;}

        public int streetNumber { get; set; }

        public string postalCode { get; set; }
    }
}
