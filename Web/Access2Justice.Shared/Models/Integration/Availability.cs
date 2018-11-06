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
        public Schedule RegularBusinessHours { get; set; }

        /// <summary>
        /// Service hours during official holidays
        /// </summary>
        public Schedule HolidayBusinessHours { get; set; }

        /// <summary>
        /// Typical wait time that client can expect to wait for services
        /// </summary>
        public TimeSpan WaitTime { get; set; }
        
    }
}