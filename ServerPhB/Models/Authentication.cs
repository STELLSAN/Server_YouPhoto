using System;

namespace ServerPhB.Models
{
    public class Authentication
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }

        public void GenerateToken(User userIn){
            // TODO: Implement this method
        }

        public void ValidateToken(string TokenIn){
            // TODO: Implement this method
        }
    }
}
