using System;

namespace Assignment
{
    public abstract class Loan
    {
        public string ApplicantName { get; set; }
        public decimal LoanAmount { get; set; }

        public Loan(string applicantName, decimal loanAmount)
        {
            ApplicantName = applicantName;
            LoanAmount = loanAmount;
        }

      
        public void VerifyDocuments()
        {
            Console.WriteLine($"Documents verified for {ApplicantName}");
        }

        public void SanctionAmount()
        {
            Console.WriteLine($"Loan Amount Sanctioned : ₹{LoanAmount}");
        }

     
        public abstract double CalculateInterestRate();

        public abstract bool CheckEligibility();
    }

    public class HomeLoan : Loan
    {
        public HomeLoan(string name, decimal amount)
            : base(name, amount)
        {
        }

        public override double CalculateInterestRate()
        {
            return 8.5;
        }

        public override bool CheckEligibility()
        {
            return LoanAmount <= 5000000;
        }
    }

    public class CarLoan : Loan
    {
        public CarLoan(string name, decimal amount)
            : base(name, amount)
        {
        }

        public override double CalculateInterestRate()
        {
            return 10.5;
        }

        public override bool CheckEligibility()
        {
            return LoanAmount <= 1500000;
        }
    }

    public class EducationLoan : Loan
    {
        public EducationLoan(string name, decimal amount)
            : base(name, amount)
        {
        }

        public override double CalculateInterestRate()
        {
            return 6.5;
        }

        public override bool CheckEligibility()
        {
            return LoanAmount <= 1000000;
        }
    }
}