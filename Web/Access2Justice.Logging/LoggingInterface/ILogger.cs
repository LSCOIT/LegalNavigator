using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.DataContracts;
using Access2Justice.Logger.AppEnum;


namespace Access2Justice.Logger.LoggingInterface
{
    /// <summary>
    /// Common logging interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Handles exception logging
        /// </summary>
        void TrackException(Exception ex, EventDictionary? eventId = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// Handles exception logging
        /// </summary>
        void TrackException(Exception ex, string corelationId, EventDictionary? eventId = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// Trace events
        /// </summary>
        void TrackTrace(string message, SeverityLevel severity, EventDictionary eventId, string corelationId, IDictionary<string, string> properties = null);

        /// <summary>
        /// Trace events
        /// </summary>
        void TrackTrace(string message, SeverityLevel severity, EventDictionary eventId, IDictionary<string, string> properties = null);

        /// <summary>
        /// Track events (business processes etc..)
        /// </summary>
        void TrackEvent(string eventName, string corelationId, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// Track events (business processes etc..)
        /// </summary>
        void TrackEvent(string eventName,  IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        ///<summary>
        /// Track Request sent to Outbound Service
        ///</summary>
        void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success);

        void TrackRequest(RequestTelemetry request);

        /// <summary>
        /// Track Metric
        /// </summary>
        void TrackMetric(string name, double value, string corelationId = null, IDictionary<string, string> properties = null);
        /// <summary>
        /// Track Metric
        /// </summary>
        void TrackMetric(string name, double value,  IDictionary<string, string> properties = null);

        /// <summary>
        /// Track Dependency
        /// </summary>
        void TrackDependency(string dependencyTypeName, string target, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, string resultCode, bool success);

        /// <summary>
        /// Track Availability
        /// </summary>
        void TrackAvailability(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null);

        /// <summary>
        /// Flush Insights
        /// </summary>
        void FlushInsights();
    }
}