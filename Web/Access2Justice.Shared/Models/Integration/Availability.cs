using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models.Integration
{
    public class Availability
    {
        /// <summary>
        /// Regular business hours
        /// </summary>
        [JsonProperty(PropertyName = "regularBusinessHours")]
        public IEnumerable<Schedule> RegularBusinessHours { get; set; }

        /// <summary>
        /// Service hours during official holidays
        /// </summary>
        [JsonProperty(PropertyName = "holidayBusinessHours")]
        public IEnumerable<Schedule> HolidayBusinessHours { get; set; }

        /// <summary>
        /// Typical wait time that client can expect to wait for services
        /// </summary>
        [JsonProperty(PropertyName = "waitTime")]
        public TimeSpan WaitTime { get; set; }
        
    }
}