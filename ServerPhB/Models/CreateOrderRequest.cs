using System;

namespace ServerPhB.Models
{
    public class CreateOrderRequest
    {
        public List<IFormFile> Photos { get; set; }
        public string Format { get; set; }
        public int Quantity { get; set; }
        public int DecorationOptionID { get; set; }
        public int DeliveryMethodID { get; set; }
        public string Address { get; set; }
        public string Comments { get; set; }
        public int TotalPrice { get; set; }
    }
}
