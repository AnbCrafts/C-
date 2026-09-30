using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    public class Vehicle
    {
        public string VehicleNumber { get; set; }
        public string Brand { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Vehicle(string number,string brand, double lat,double lng){
            VehicleNumber = number;
            Brand = brand;
            Latitude = lat;
            Longitude = lng;
            
            }
        public void LogGPS()
        {
            Console.WriteLine($"Location: {Latitude}, {Longitude}");
        }

        public  string getBrand()
        {
            return Brand;
        }

        public string getVehicleNumber()
        {
            return VehicleNumber;
        }
        
        public virtual void getVehicleDetails()
        {
            Console.WriteLine($"vehicle Number - ${this.getVehicleNumber()} \n brand - {this.getBrand()} \n ");
            LogGPS();
        }

       

    }

    public class MotorizedVehicle : Vehicle
    {
        protected string FuelType { get; set; }
        protected bool EngineRunning { get; set; }
        protected int HorsePower { get; set; }
        public MotorizedVehicle(string number, string brand, double lat, double lng, string fuelType, bool engineRunning, int horsePower):base( number,  brand,  lat,  lng)
        {
            FuelType = fuelType;
            EngineRunning = engineRunning;
            HorsePower = horsePower;
        }


        public void StartEngine()
        {
            EngineRunning = true;
            Console.WriteLine("Engine Started");
        }
    }

    public class ElectricTruck : MotorizedVehicle
    {
        public double BatteryCapacity { get; set; }
        public double CurrentCharge { get; set; }
        public double Efficiency { get; set; }
        public ElectricTruck(string number, string brand, double lat, double lng, string fuelType, bool engineRunning, int horsePower, double batteryCapacity, double currentCharge, double efficiency) : base(number, brand, lat, lng,fuelType,engineRunning,horsePower)
        {
            BatteryCapacity = batteryCapacity;
            CurrentCharge = currentCharge;
            Efficiency = efficiency;
        }
        public double CalculateRange()
        {
            return CurrentCharge * Efficiency;
        }
    }
}
