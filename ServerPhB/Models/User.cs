using System;

namespace ServerPhB.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int Role { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
