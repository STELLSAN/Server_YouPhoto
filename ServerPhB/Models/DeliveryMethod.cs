using System;

namespace ServerPhB.Models
{
    public class DeliveryMethod
    {
        public int DeliveryMethodID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Cost { get; set; }
    }
}
