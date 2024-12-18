using System;

namespace ServerPhB.Models
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public string ClientID { get; set; }
        public string DeliveryMethodID { get; set; }
        public string Address { get; set; }
        public int DecorationOptionID { get; set; }
        public string Comments { get; set; }
        public int TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
