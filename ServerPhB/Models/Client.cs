using System;

namespace ServerPhB.Models
{
    public class Client : User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
