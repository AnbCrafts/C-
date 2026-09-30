using System;

namespace Assignment
{

    public interface INavigable
    {
        void Navigate(string destination);
    }

    public interface IChargeable
    {
        void ChargeBattery();
    }

    public interface ISensorReadable
    {
        void ReadSensors();
    }


    public abstract class BaseRobot
    {
        public string RobotId { get; set; }

        public BaseRobot(string robotId)
        {
            RobotId = robotId;
        }

        public void StartRobot()
        {
            Console.WriteLine($"Robot {RobotId} Started");
        }

        public abstract void PerformTask();
    }


    public class WarehouseRobot :
        BaseRobot,
        INavigable,
        IChargeable,
        ISensorReadable
    {
        public WarehouseRobot(string robotId)
            : base(robotId)
        {
        }

        public override void PerformTask()
        {
            Console.WriteLine("Moving inventory in warehouse");
        }

        public void Navigate(string destination)
        {
            Console.WriteLine($"Navigating to {destination}");
        }

        public void ChargeBattery()
        {
            Console.WriteLine("Charging Battery");
        }

        public void ReadSensors()
        {
            Console.WriteLine("Reading Proximity Sensors");
        }
    }
}