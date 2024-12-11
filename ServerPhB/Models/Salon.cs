using System;

namespace ServerPhB.Models
{
    public class Salon
    {
        public int SalonID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Equipment> EquipmentList { get; set; }
    }
}
