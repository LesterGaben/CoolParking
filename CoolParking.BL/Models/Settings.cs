using System;

namespace CoolParking.BL.Models
{
    public static class Settings
    {
        public static decimal InitialParkingBalance { get; } = 0;
        public static int ParkingCapacity { get; } = 10;
        public static int PaymentDeductionPeriodSeconds { get; } = 5;
        public static int LogWritePeriodSeconds { get; } = 60;

        public static decimal GetTariff(VehicleType vehicleType)
        {
            switch (vehicleType)
            {
                case VehicleType.PassengerCar:
                    return 2;
                case VehicleType.Truck:
                    return 5;
                case VehicleType.Bus:
                    return 3.5m;
                case VehicleType.Motorcycle:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(vehicleType), vehicleType, "Invalid vehicle type.");
            }
        }

        public static decimal PenaltyCoefficient { get; } = 2.5M;
    }
}
