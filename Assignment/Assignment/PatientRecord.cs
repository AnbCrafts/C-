using System;

namespace Assignment
{
    internal class PatientRecord
    {
        private string healthStats;

     
        public string Name { get; private set; }

    
        public decimal Bill { get; private set; }

        public bool BillingStatus { get; private set; }

        public PatientRecord(string name, decimal bill)
        {
            Name = name;
            Bill = bill;
            BillingStatus = false;
            healthStats = "Healthy";
        }

        public void UpdateHealthRecord(string record)
        {
            healthStats = record;
        }

        public string HealthRecord
        {
            get { return healthStats; }
        }

        public void MarkBillAsPaid()
        {
            BillingStatus = true;
        }

        public void GetDetails()
        {
            string note = BillingStatus ? "" : "not";

            Console.WriteLine($"Patient Name : {Name}");
            Console.WriteLine($"Bill Amount  : {Bill}");
            Console.WriteLine($"Bill is {note} paid");
            Console.WriteLine($"Health Stats : {HealthRecord}");
        }
    }
}