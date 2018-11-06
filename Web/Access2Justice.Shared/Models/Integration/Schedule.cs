using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models.Integration
{
    public enum Weekday
    {
        Sunday = 0,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }

    public class Schedule
    {
        public Weekday Day { get; set; }
                
        public TimeSpan OpensAt { get; set; }

        public TimeSpan ClosesAt { get; set; }
    }
}