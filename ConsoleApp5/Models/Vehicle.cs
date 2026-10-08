using System;

namespace ConsoleApp5.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public decimal OdometerReading { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public Vehicle()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
        }

        public Vehicle(string vin, string manufacturer, string model, decimal odometerReading, bool isActive = true)
        {
            VIN = vin;
            Manufacturer = manufacturer;
            Model = model;
            OdometerReading = odometerReading;
            IsActive = isActive;
            CreatedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {Id,-4} | VIN: {VIN,-18} | Make: {Manufacturer,-12} | Model: {Model,-12} | Odo: {OdometerReading,8:N0} km | Active: {IsActive,-5} | Created: {CreatedDate:yyyy-MM-dd HH:mm}";
        }
    }
}
