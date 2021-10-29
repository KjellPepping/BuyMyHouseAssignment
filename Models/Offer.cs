using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Offer : TableEntity
    {
        public int offerId { get; set; }
        public User user { get; set; }
        public ValueTuple<double,double> budgetRange { get; set; }
        public IEnumerable<House> offerHouses { get; set; }
    }
}
