using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Access2Justice.Logger.AppEnum;
using Access2Justice.Logger.LoggingInterface;
using Access2Justice.Logger.Utility;

namespace Access2Justice.Logger.Logging
{
    /// <summary>
    /// Application Insights implementation for ILogger
    /// </summary>
    public class AppInsightsLogger : ILogger
    {
        private TelemetryClient aiClient;


        private AppInsightsLogger(string instrumentationKey)
        {
            aiClient = new TelemetryClient();
            aiClient.InstrumentationKey = instrumentationKey;
        }

        private static Object lockObject = new Object();
        private static Lazy<IDictionary<String, AppInsightsLogger>> aiLogger = new Lazy<IDictionary<string, AppInsightsLogger>>(
            () => new Dictionary<String, AppInsightsLogger>()
            );

        public static AppInsightsLogger GetInstance(string instrumentationKey)
        {
            lock (lockObject)
            {
                if (aiLogger.Value.ContainsKey(instrumentationKey.Trim()))
                {
                    return aiLogger.Value[instrumentationKey];
                }
                else
                {
                    var appInsightsLogger = new AppInsightsLogger(instrumentationKey.Trim());
                    aiLogger.Value.Add(instrumentationKey.Trim(), appInsightsLogger);
                    return appInsightsLogger;
                }
            }
        }

        public virtual void TrackException(Exception ex, string corelationId, EventDictionary? eventId = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            properties = InitializeProperties(properties);

            if (eventId != null)
            {
                properties.AddEventId((int)eventId);
            }
            else
            {
                properties.AddEventId(ex.GetEventId());
            }
            if (corelationId != null)
            {
                properties.AddCorelationId(corelationId);
            }

            aiClient.TrackException(ex, properties, metrics);
        }

        public virtual void TrackException(Exception ex, EventDictionary? eventId = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            properties = InitializeProperties(properties);

            if (eventId != null)
            {
                properties.AddEventId((int)eventId);
            }
            else
            {
                properties.AddEventId(ex.GetEventId());
            }

            aiClient.TrackException(ex, properties, metrics);
        }


        public virtual void TrackTrace(string message, SeverityLevel severity, EventDictionary eventId, string corelationId, IDictionary<string, string> properties = null)
        {
            //TelemetryClient client = new TelemetryClient();
            properties = InitializeProperties(properties);
            properties.AddEventId((int)eventId);
            if (corelationId != null)
            {
                properties.AddCorelationId(corelationId);
            }
            aiClient.TrackTrace(message, severity, properties);
        }


        public virtual void TrackTrace(string message, SeverityLevel severity, EventDictionary eventId, IDictionary<string, string> properties = null)
        {
            //TelemetryClient client = new TelemetryClient();
            properties = InitializeProperties(properties);
            properties.AddEventId((int)eventId);

            aiClient.TrackTrace(message, severity, properties);
        }

        public virtual void TrackEvent(string eventName, string corelationId, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            //TelemetryClient client = new TelemetryClient();
            properties = InitializeProperties(properties);
            if (corelationId != null)
            {
                properties.AddCorelationId(corelationId);
            }
            aiClient.TrackEvent(eventName, properties, metrics);
        }

        public virtual void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            //TelemetryClient client = new TelemetryClient();
            aiClient.TrackEvent(eventName, properties, metrics);
        }

        public virtual void TrackMetric(string name, double value, string corelationId, IDictionary<string, string> properties = null)
        {
            //TelemetryClient client = new TelemetryClient();
            properties = InitializeProperties(properties);
            if (corelationId != null)
            {
                properties.AddCorelationId(corelationId);
            }
            aiClient.TrackMetric(name, value, properties);
        }

        public virtual void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            //TelemetryClient client = new TelemetryClient();
            aiClient.TrackMetric(name, value, properties);
        }

        public virtual void TrackDependency(string dependencyTypeName, string target, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, string resultCode, bool success)
        {
            //TelemetryClient client = new TelemetryClient();
            aiClient.TrackDependency(dependencyTypeName, target, dependencyName, data, startTime, duration, resultCode, success);
        }

        public virtual void TrackAvailability(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null)
        {
            //TelemetryClient client = new TelemetryClient();
            aiClient.TrackAvailability(name, timeStamp, duration, runLocation, success, message);
        }

        private IDictionary<string, string> InitializeProperties(IDictionary<string, string> properties)
        {
            return properties ?? new Dictionary<string, string>();
        }

        public void FlushInsights()
        {
            aiClient.Flush();
            // Allow time for flushing:
            System.Threading.Thread.Sleep(1000);
        }

        public void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            aiClient.TrackRequest(name, startTime, duration, responseCode, success);
        }

        public void TrackRequest(RequestTelemetry request)
        {
            aiClient.TrackRequest(request);
        }
    }

}