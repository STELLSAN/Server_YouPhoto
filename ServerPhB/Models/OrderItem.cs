using System;

namespace ServerPhB.Models
{
    public class OrderItem : Photo
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        // PhotoID is inherited from Photo
        public int DecorationOptions { get; set; }
        public int Quantity { get; set; }
    }
}
