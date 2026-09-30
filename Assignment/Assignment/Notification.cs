using System;
using System.Collections.Generic;

namespace Assignment
{
    public class Notification
    {
        public virtual void SendNotification(string message)
        {
            Console.WriteLine($"Sending Notification : {message}");
        }
    }

    public class EmailNotification : Notification
    {
        public override void SendNotification(string message)
        {
            Console.WriteLine($"Email Sent : {message}");
        }
    }

    public class SMSNotification : Notification
    {
        public override void SendNotification(string message)
        {
            Console.WriteLine($"SMS Sent : {message}");
        }
    }

    public class WhatsAppNotification : Notification
    {
        public override void SendNotification(string message)
        {
            Console.WriteLine($"WhatsApp Message Sent : {message}");
        }
    }

    public class PushNotification : Notification
    {
        public override void SendNotification(string message)
        {
            Console.WriteLine($"Push Notification Sent : {message}");
        }
    }
}