using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Access2Justice.Logger.AppEnum;
using Access2Justice.Logger.LoggingInterface;
using Access2Justice.Logger.UtilityComponent;

namespace Access2Justice.Logger.Logging
{
	/// <summary>
	/// Application Insights implementation for ILogger
	/// </summary>
	public class AppInsightsLogger : ILogger
    {
        private TelemetryClient _client;

        
        private AppInsightsLogger(string instrumentationKey)
        {
            _client = new TelemetryClient();
            _client.InstrumentationKey = instrumentationKey;
        }

        private static Object _object = new Object();
        private static Lazy<IDictionary<String, AppInsightsLogger>> _loggers = new Lazy<IDictionary<string, AppInsightsLogger>>(
            ()=>new Dictionary<String, AppInsightsLogger>()
            );

        public static AppInsightsLogger GetInstance(string instrumentationKey)
        {
            lock (_object) {
                if (_loggers.Value.ContainsKey(instrumentationKey.Trim()))
                {
                    return _loggers.Value[instrumentationKey];
                }
                else
                {
                    var appInsightsLogger = new AppInsightsLogger(instrumentationKey.Trim());
                    _loggers.Value.Add(instrumentationKey.Trim(), appInsightsLogger);
                    return appInsightsLogger;
                }
            }
        }

        public virtual void TrackException(Exception ex, string corelationId,  EventDictionary? eventId = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
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

            _client.TrackException(ex, properties, metrics);
        }

        public virtual void TrackException(Exception ex,  EventDictionary? eventId = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
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
            
            _client.TrackException(ex, properties, metrics);
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
            _client.TrackTrace(message, severity, properties);
        }


        public virtual void TrackTrace(string message, SeverityLevel severity, EventDictionary eventId, IDictionary<string, string> properties = null)
        {
            //TelemetryClient client = new TelemetryClient();
            properties = InitializeProperties(properties);
            properties.AddEventId((int)eventId);
            
            _client.TrackTrace(message, severity, properties);
        }

        public virtual void TrackEvent(string eventName, string corelationId, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            //TelemetryClient client = new TelemetryClient();
            properties = InitializeProperties(properties);
            if (corelationId != null)
            {
                properties.AddCorelationId(corelationId);
            }
            _client.TrackEvent(eventName, properties, metrics);
        }

        public virtual void TrackEvent(string eventName,  IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            //TelemetryClient client = new TelemetryClient();
            _client.TrackEvent(eventName, properties, metrics);
        }

        public virtual void TrackMetric(string name, double value, string corelationId, IDictionary<string, string> properties = null)
        {
            //TelemetryClient client = new TelemetryClient();
            properties = InitializeProperties(properties);
            if (corelationId != null)
            {
                properties.AddCorelationId(corelationId);
            }
            _client.TrackMetric(name, value, properties);
        }

        public virtual void TrackMetric(string name, double value,  IDictionary<string, string> properties = null)
        {
            //TelemetryClient client = new TelemetryClient();
            _client.TrackMetric(name, value, properties);
        }

        public virtual void TrackDependency(string dependencyTypeName, string target, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, string resultCode, bool success)
        {
            //TelemetryClient client = new TelemetryClient();
            _client.TrackDependency(dependencyTypeName, target, dependencyName, data, startTime, duration, resultCode, success);
        }

        public virtual void TrackAvailability(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null)
        {
            //TelemetryClient client = new TelemetryClient();
            _client.TrackAvailability(name, timeStamp, duration, runLocation, success, message);
        }

        private IDictionary<string, string> InitializeProperties(IDictionary<string, string> properties)
        {
            return properties ?? new Dictionary<string, string>();
        }

        public void FlushInsights()
        {
            _client.Flush();
            // Allow time for flushing:
            System.Threading.Thread.Sleep(1000);
        }

        public void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            _client.TrackRequest(name, startTime, duration, responseCode, success);
        }

        public void TrackRequest(RequestTelemetry request)
        {
            _client.TrackRequest(request);
        }
    }
}