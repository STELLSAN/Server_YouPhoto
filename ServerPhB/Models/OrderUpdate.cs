using System;

namespace ServerPhB.Models
{
    public class OrderUpdate
    {
        public int UpdateID { get; set; }
        public int OrderID { get; set; }
        public string Status { get; set; }
    }
}
