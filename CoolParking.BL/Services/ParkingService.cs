using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Timers;

namespace CoolParking.BL.Services
{
    public class ParkingService : IParkingService, IDisposable
    {
        readonly ITimerService withdrawTimer;
        readonly ITimerService logTimer;
        readonly ILogService logService;
        readonly List<TransactionInfo> transactions = new List<TransactionInfo>();
        readonly Parking parking;
        public ParkingService(ITimerService _withdrawTimer, ITimerService _logTimer, ILogService _logService) 
        {
            parking = Parking.getInstance(Settings.InitialParkingBalance);
            withdrawTimer = _withdrawTimer;
            logTimer = _logTimer;
            logService = _logService;
            withdrawTimer.Elapsed += WithdrawTimerElapsed;


            logTimer.Elapsed += LogTimerElapsed;
            _logTimer.Elapsed += (sender, e) => _logService.Write("LogTimer has elapsed");

            _withdrawTimer.Interval = Settings.PaymentDeductionPeriodSeconds * 1000;
            _logTimer.Interval=Settings.LogWritePeriodSeconds * 1000;
            withdrawTimer.Start();
            logTimer.Start();
        }
        public void AddVehicle(Vehicle vehicle)
        {
            if (GetFreePlaces() == 0)
            {
                throw new InvalidOperationException();
            }
            else if(parking.vehicles.Any(x => x.Id == vehicle.Id))
            {
                throw new ArgumentException("There are vehicle with same id.");
            }
            else
            {
                parking.vehicles.Add(vehicle);
            }

        }

        public void Dispose()
        {
            parking.Balance = Settings.InitialParkingBalance;
            parking.vehicles.Clear();
            withdrawTimer.Dispose();
        }

        public decimal GetBalance()
        {
            return parking.Balance;
        }

        public int GetCapacity()
        {
            return Settings.ParkingCapacity;
        }

        public int GetFreePlaces()
        {
            if (parking.vehicles==null)
            {
                return Settings.ParkingCapacity;
            }
            else
            {
                return (Settings.ParkingCapacity - parking.vehicles.Count(x => x != null));
            }
        }
           
        public TransactionInfo[] GetLastParkingTransactions()
        {
            return transactions.ToArray();
        }

        public ReadOnlyCollection<Vehicle> GetVehicles()
        {
            return new ReadOnlyCollection<Vehicle>(parking.vehicles);
        }

        public string ReadFromLog()
        {
            return logService.Read();
        }

        public void RemoveVehicle(string vehicleId)
        {
            try
            {
                parking.vehicles.Remove(parking.vehicles.First(x => x.Id == vehicleId));
            }
            catch 
            {
                throw new ArgumentException("No vehicle with such id.");
            }
        }

        public void TopUpVehicle(string vehicleId, decimal sum)
        {
            try
            {
                if (sum < 0) throw new ArgumentException("Sum<0.");
                parking.vehicles.First(x => x.Id == vehicleId).Balance += sum;
            }
            catch(Exception)
            {
                throw new ArgumentException("No vehicle with such id.");
            }
        }
        private void WithdrawTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var vehicles = parking.vehicles;
            foreach (var vehicle in vehicles)
            {
                decimal tariff = Settings.GetTariff(vehicle.VehicleType);
                decimal deduction = tariff;
                vehicle.Balance -= deduction;
                parking.Balance += deduction;
                var transaction = new TransactionInfo(DateTime.Now, vehicle.Id, deduction);
                transactions.Add(transaction);
            }
        }

        private void LogTimerElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var transaction in transactions)
            {
                logService.Write(transaction.ToString());
            }
            transactions.Clear();
        }

    }
}