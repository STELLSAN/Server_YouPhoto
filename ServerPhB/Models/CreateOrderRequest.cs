using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ServerPhB.Models
{
    public class CreateOrderRequest
    {
        public List<IFormFile> Photos { get; set; }
        public string Format { get; set; }
        public int Quantity { get; set; }
        public int DecorationOptionID { get; set; }
        public int DeliveryMethodID { get; set; }
        public string? Address { get; set; } // Allow null values
        public string? Comments { get; set; } // Allow null values
        public int TotalPrice { get; set; }
    }
}
