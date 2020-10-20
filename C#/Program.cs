using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;


namespace Lab3
{
    public class Program
    {
        static void Main(string[] args)
        {
            Calendar Cal = new Calendar();
            Event ev1 = new meetingEvent("10/25/2020","Modul", "12:00","15:30","google.meet");
            Event ev2 = new birthdayEvent("09/26/2020", "HP", "Sasha");
            Cal.AddEvent(ev1);
            Cal.AddEvent(ev2);
            Console.WriteLine("Copy Test: Original Event:");
            Event ev3 = ev1.DeepCopy();
            ev1.Print();
            Console.WriteLine("Copied Event: ");
            ev3.Print();
            Console.WriteLine("Change the copied event data to new: (10/29/2020)");
            ev3.date = Convert.ToDateTime("10/29/2020");
            ev3.Print();
            Cal.AddEvent(ev3);
            Console.WriteLine("Calendar output: ");
            Cal.Print();
        }
    }

    public class Calendar
    {
        static List<Event> EventList = new List<Event>();

        public void AddEvent(Event obj)
        {
            EventList.Add(obj);
        }
        public void Print()
        {
            // sort by date
            EventList.Sort((x, y) => DateTime.Compare(x.date, y.date));

            foreach (var _event in EventList)
            {
                _event.Print();
            }
        }
    }

    public class Event
    {
        public DateTime date;
        public string description;
        // private secretKey
        private protected string secretKey;

        public Event(string eventDate, string eventDescription)
        {
            date = Convert.ToDateTime(eventDate);
            description = eventDescription;
            secretKey = GenerateRandomCryptographicKey(8);
        }

        public virtual void Print()
        {
            Console.WriteLine("\nDate: {0}\nEvent: {1}", date.ToString("D"), description);
        }
        
        public Event DeepCopy()
        {
            Event clone = (Event) this.MemberwiseClone();
            return clone;
        }
        // function to create a secret key
        private string GenerateRandomCryptographicKey(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }

    public class meetingEvent : Event
    {
        DateTime start;
        DateTime end;
        string place;

        public meetingEvent(string eventDate, 
                            string eventDescription, 
                            string startTime, 
                            string endTime, 
                            string place_) : base(eventDate, eventDescription)
        {
            start = DateTime.ParseExact(startTime, "HH:mm",
                                        CultureInfo.InvariantCulture);
            end = DateTime.ParseExact(endTime, "HH:mm",
                                        CultureInfo.InvariantCulture);
            place = place_;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine("Start Time: {0}\nEnd Time: {1}\nPlace: {2}\n",
                            start.ToString("hh:mm tt"), end.ToString("hh:mm tt"), place);
        }
    }

    public class birthdayEvent : Event
    {
        string birthdayPerson;
        public birthdayEvent(string eventDate, 
                             string eventDescription, 
                             string bPersonData) : base(eventDate, eventDescription)
        {
            birthdayPerson = bPersonData;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine("Birthday person: {0}", birthdayPerson);
        }
    }
}
