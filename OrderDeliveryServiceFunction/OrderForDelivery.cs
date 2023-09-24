using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDeliveryServiceFunction
{
    public class OrderForDelivery
    {
        public int OrderId { get; set; }
        public string ShippingAddress { get; set; }
        public List<string> Items { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
