using System;

namespace CoolParking.BL.Models
{
    public struct TransactionInfo
    {
        public DateTime Time { get; set; }
        public string VehicleId { get; set; }
        public decimal Sum { get; set; }
        public TransactionInfo(DateTime time, string vehicleId, decimal sum)
        {
            Time = time;
            VehicleId = vehicleId;
            Sum = sum;
        }
        override public string ToString()
        {
           return $"{Time}|{VehicleId}|{Sum}";
        }
    }
}