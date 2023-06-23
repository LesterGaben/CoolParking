using System;
using System.Text.RegularExpressions;

namespace CoolParking.BL.Models
{
    public class Vehicle
    {
        public string Id { get; }
        public VehicleType VehicleType { get; }
        public decimal Balance { get; set; }
        public Vehicle(string id, VehicleType vehicleType, decimal balance)
        {
            if ( !Regex.IsMatch(id, @"^[A-Z]{2}-\d{4}-[A-Z]{2}$")|| balance<0)
            {
                throw new ArgumentException("Wrong balance or Id.");
            }
            Id = id;
            VehicleType = vehicleType;
            Balance = balance;
        }
        public static string GenerateRandomRegistrationPlateNumber()
        {
            Random random = new Random();

            char letter1 = (char)(random.Next(65,90));
            char letter2 = (char)(random.Next(65, 90));

            string digits = random.Next(10000).ToString("D4");

            char letter3 = (char)(random.Next(65, 90));
            char letter4 = (char)(random.Next(65, 90));

            return $"{letter1}{letter2}-{digits}-{letter3}{letter4}";
        }
    }
}
