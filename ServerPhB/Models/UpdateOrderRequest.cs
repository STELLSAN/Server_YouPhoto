using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ServerPhB.Models
{
    public class UpdateOrderRequest
    {
        public int OrderID { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public int? DecorationOptionID { get; set; }
        public string Comments { get; set; }
        public int? TotalPrice { get; set; }
        public int? Quantity { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
