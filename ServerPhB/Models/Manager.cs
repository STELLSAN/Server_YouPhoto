using System;

namespace ServerPhB.Models
{
    public class Manager
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public void AssignOrderToSalon(Order orderIn, Salon salonIn){
            // TODO: Implement this method
        }

        public void TrackOrder(int orderID){
            // TODO: Implement this method
        }

        public void ViewOrderList(){
            // TODO: Implement this method
        }
    }
}
