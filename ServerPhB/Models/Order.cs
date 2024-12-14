using System;

namespace ServerPhB.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public string ClientID { get; set; }
        public string DeliveryMethodID { get; set; }
        public string Address { get; set; }

    }
}
