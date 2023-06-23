using System.Collections.Generic;

namespace CoolParking.BL.Models
{
    public class Parking
    {
        private static Parking instance;
        public decimal Balance { get; set; }
        public List<Vehicle> vehicles { get; set; }
        protected Parking(decimal _balance) {
        Balance = _balance;
            vehicles = new List<Vehicle>();
        }
        public static Parking getInstance(decimal balance)
        {
            if (instance == null)
                instance = new Parking(balance);
            return instance;
        }
    }
}

