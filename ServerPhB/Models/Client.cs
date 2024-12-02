using System;

namespace ServerPhB.Models
{
    public class Client : User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public void UploadPhoto(Photo photoIn){
            // TODO: Implement this method
        }

        public void CreateOrder(Order orderIn){
            // TODO: Implement this method
        }

        public void TrackOrder(Order orderIn){
            // TODO: Implement this method
        }
    }
}
