using System;
using System.Collections.Generic;

namespace ServerPhB.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public string ClientID { get; set; }
        public string DeliveryMethodID { get; set; }
        public string? Address { get; set; } // Nullable
        public int DecorationOptionID { get; set; }
        public string? Comments { get; set; } // Nullable
        public int TotalPrice { get; set; }
        public int Quantity { get; set; }
        public List<string> PhotoPaths { get; set; }
    }
}
