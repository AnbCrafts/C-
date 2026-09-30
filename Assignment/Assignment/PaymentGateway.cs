using System;

class PaymentProcessor
{

    private string cardNumber;
    private decimal accountBalance;
    private string securityToken;

   
    public string CardNumber
    {
        set
        {
            cardNumber = value;
        }

        get
        {
            return "************" +
                   cardNumber.Substring(cardNumber.Length - 4);
        }
    }


    public decimal AccountBalance
    {
        get
        {
            return accountBalance;
        }

        set
        {
            if (value >= 0)
            {
                accountBalance = value;
            }
            else
            {
                Console.WriteLine("Negative balance not allowed.");
            }
        }
    }

    public string SecurityToken
    {
        set
        {
            securityToken = value;
        }
    }
}

