namespace Assignment
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal BaseSalary { get; set; }

        public Employee(int id, string name,
                        string address, decimal salary)
        {
            Id = id;
            Name = name;
            Address = address;
            BaseSalary = salary;
        }

        public virtual decimal GetSalary()
        {
            return BaseSalary;
        }

        public virtual void Display()
        {
            Console.WriteLine($"ID : {Id}");
            Console.WriteLine($"Name : {Name}");
            Console.WriteLine($"Salary : {GetSalary()}");
        }
    }
    public class FullTimeEmployee : Employee
    {
        public decimal HRA { get; set; }

        public FullTimeEmployee(int id,
                                string name,
                                string address,
                                decimal salary,
                                decimal hra)
            : base(id, name, address, salary)
        {
            HRA = hra;
        }

        public override decimal GetSalary()
        {
            return BaseSalary + HRA;
        }
    }
    public class ContractEmployee : Employee
    {
        public decimal Bonus { get; set; }

        public ContractEmployee(int id,
                                string name,
                                string address,
                                decimal salary,
                                decimal bonus)
            : base(id, name, address, salary)
        {
            Bonus = bonus;
        }

        public override decimal GetSalary()
        {
            return BaseSalary + Bonus;
        }
    }

    public class Freelancer : Employee
    {
        public int HoursWorked { get; set; }
        public decimal RatePerHour { get; set; }

        public Freelancer(int id,
                          string name,
                          string address,
                          int hours,
                          decimal rate)
            : base(id, name, address, 0)
        {
            HoursWorked = hours;
            RatePerHour = rate;
        }

        public override decimal GetSalary()
        {
            return HoursWorked * RatePerHour;
        }
    }

}