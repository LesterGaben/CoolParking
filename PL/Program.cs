using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.BL.Services;
using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1 {
    public class Program {
        public static void Main() {
            var app = new ParkingApp();
            app.Run();
        }
    }

    public class ParkingApp {
        private ITimerService TimerService;
        private ILogService LogService;
        private ParkingService Parking;

        public ParkingApp() {
            TimerService = new TimerService();
            LogService = new LogService();
            Parking = new ParkingService(TimerService, TimerService, LogService);
        }

        public void Run() {
            while (true) {
                PrintMenu();
                string input = Console.ReadLine();
                if (IsInputValid(input, out int choice)) {
                    switch (choice) {
                        case 1:
                            ShowParkingBalance();
                            break;
                        case 2:
                            ShowParkingCapacity();
                            break;
                        case 3:
                            ShowFreePlaces();
                            break;
                        case 4:
                            ShowVehicles();
                            break;
                        case 5:
                            AddVehicle();
                            break;
                        case 6:
                            RemoveVehicle();
                            break;
                        case 7:
                            ShowParkingTransactions();
                            break;
                        case 8:
                            TopUpVehicle();
                            break;
                        case 10:
                            return;
                    }
                }
                else {
                    Console.WriteLine("Invalid option. Try again.");
                }
            }
        }

        private void PrintMenu() {
            Console.WriteLine("\n\t\tMain Menu");
            Console.WriteLine("\t 1 - Show Parking Balance\n\t 2 - Show Parking Capacity" +
                "\n\t 3 - Show Free Parking Spaces\n\t 4 - Show Parked Vehicles" +
                "\n\t 5 - Add Vehicle\n\t 6 - Remove Vehicle" +
                "\n\t 7 - Show Parking Transactions\n\t 8 - Top Up Vehicle Balance" +
                "\n\t 10 - Exit");
        }

        private bool IsInputValid(string input, out int choice) {
            return Int32.TryParse(input, out choice) && (choice >= 1 && choice <= 10);
        }

        private void ShowParkingBalance() {
            decimal balance = Parking.GetBalance();
            Console.WriteLine($"\nParking balance: {balance}");
        }

        private void ShowParkingCapacity() {
            int capacity = Parking.GetCapacity();
            Console.WriteLine($"\nParking capacity: {capacity}");
        }

        private void ShowFreePlaces() {
            int freePlaces = Parking.GetFreePlaces();
            Console.WriteLine($"\nFree parking spaces: {freePlaces}");
        }

        private void ShowVehicles() {
            var vehicles = Parking.GetVehicles();
            if (vehicles.Count == 0) {
                Console.WriteLine("\nNo vehicles parked.");
            }
            else {
                Console.WriteLine("\nParked Vehicles: ");
                foreach (var vehicle in vehicles) {
                    Console.WriteLine($"ID: {vehicle.Id}, Type: {vehicle.VehicleType}, Balance: {vehicle.Balance}");
                }
            }
        }

        private void AddVehicle() {
            Console.Write("\nEnter vehicle balance: ");
            decimal balance = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Select vehicle type (1- Passenger Car, 2- Truck, 3- Bus, 4- Motorcycle): ");
            int vehicleTypeInput = Convert.ToInt32(Console.ReadLine());

            VehicleType vehicleType = (VehicleType)(vehicleTypeInput - 1);

            Parking.AddVehicle(new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), vehicleType, balance));
            Console.WriteLine("\nVehicle added successfully.");
        }

        private void RemoveVehicle() {
            while (true) {
                Console.Write("\nEnter vehicle id: ");
                string id = Console.ReadLine();

                try {
                    Parking.RemoveVehicle(id);
                    Console.WriteLine("\nVehicle removed successfully.");
                    break;
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Try again.");
                }
            }
        }


        private void ShowParkingTransactions() {
            var transactions = Parking.GetLastParkingTransactions();
            if (transactions.Length == 0) {
                Console.WriteLine("\nNo transactions during this period.");
            }
            else {
                Console.WriteLine("\nTransactions during this period: ");
                foreach (var transaction in transactions) {
                    Console.WriteLine(transaction);
                }
            }
        }

        private void TopUpVehicle() {
            Console.Write("\nEnter vehicle id: ");
            string id = Console.ReadLine();

            Console.Write("Enter top up amount: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            Parking.TopUpVehicle(id, amount);
            Console.WriteLine("\nBalance topped up successfully.");
        }
    }
}
