using System;

namespace ServerPhB.Models
{
    public class Authentication
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
