using System;

namespace Assignment
{
    public abstract class SmartDevice
    {
        public string DeviceName { get; set; }

        public SmartDevice(string deviceName)
        {
            DeviceName = deviceName;
        }

        public abstract void TurnOn();

        public abstract void TurnOff();

        public abstract double GetEnergyConsumption();
    }

    public class SmartLight : SmartDevice
    {
        public SmartLight(string name) : base(name) { }

        public override void TurnOn()
        {
            Console.WriteLine($"{DeviceName} Light Turned ON");
        }

        public override void TurnOff()
        {
            Console.WriteLine($"{DeviceName} Light Turned OFF");
        }

        public override double GetEnergyConsumption()
        {
            return 10;
        }
    }

    public class SmartThermostat : SmartDevice
    {
        public SmartThermostat(string name) : base(name) { }

        public override void TurnOn()
        {
            Console.WriteLine($"{DeviceName} Thermostat Turned ON");
        }

        public override void TurnOff()
        {
            Console.WriteLine($"{DeviceName} Thermostat Turned OFF");
        }

        public override double GetEnergyConsumption()
        {
            return 25;
        }
    }

    public class SmartAC : SmartDevice
    {
        public SmartAC(string name) : base(name) { }

        public override void TurnOn()
        {
            Console.WriteLine($"{DeviceName} AC Turned ON");
        }

        public override void TurnOff()
        {
            Console.WriteLine($"{DeviceName} AC Turned OFF");
        }

        public override double GetEnergyConsumption()
        {
            return 120;
        }
    }
}