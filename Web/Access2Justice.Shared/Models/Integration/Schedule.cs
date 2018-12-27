using Newtonsoft.Json;
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
        [JsonProperty(PropertyName = "day")]
        public Weekday Day { get; set; }

        [JsonProperty(PropertyName = "opensAt")]
        public TimeSpan OpensAt { get; set; }

        [JsonProperty(PropertyName = "closesAt")]
        public TimeSpan ClosesAt { get; set; }
    }
}