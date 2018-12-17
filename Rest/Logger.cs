
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Rest
{
    /// <summary>
    /// ILogger extension methods for logs into application insights of different categories
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Telementry client to track different categories in application insights
        /// </summary>
        public static readonly TelemetryClient Telemetry = new TelemetryClient();

        /// <summary>
        /// Track Request in application insights, Supply user id,device id and Ip if details are exists
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="requestTelemetry">Request Telemtry object</param>
        /// <param name="userid">user id </param>
        /// <param name="deviceId">device id</param>
        /// <param name="Ip">Ip of device from where request arrived</param>
        /// <returns></returns>
        public static void TrackRequest(RequestTelemetry requestTelemetry, string userid = null, string deviceId = null,string Ip = null)
        {
            Telemetry.Context.User.Id = userid ?? string.Empty;
            Telemetry.Context.Device.Id = deviceId ?? string.Empty;
            Telemetry.Context.Location.Ip = Ip ?? string.Empty;
            Telemetry.TrackRequest(requestTelemetry);
        }

        /// <summary>
        /// Track Exception in application insights 
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="exceptionTelemetry">Exception Telemetry</param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <param name="Ip">Ip of device from where request arrived</param>
        /// <returns></returns>
        public static void TrackException(ExceptionTelemetry exceptionTelemetry, string userid = null, string deviceId = null, string Ip = null)
        {
            Telemetry.Context.User.Id = userid ?? string.Empty;
            Telemetry.Context.Device.Id = deviceId ?? string.Empty;
            Telemetry.Context.Location.Ip = Ip ?? string.Empty;
            Telemetry.TrackException(exceptionTelemetry);
        }

        /// <summary>
        /// Track Trace in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="traceTelemetry">Tracetelemetry object</param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <param name="Ip">Ip of device from where request arrived</param>
        /// <returns></returns>
        public static void TrackTrace(TraceTelemetry traceTelemetry, string userid = null, string deviceId = null, string Ip = null)
        {
            Telemetry.Context.User.Id = userid ?? string.Empty;
            Telemetry.Context.Device.Id = deviceId ?? string.Empty;
            Telemetry.Context.Location.Ip = Ip ?? string.Empty;
            Telemetry.TrackTrace(traceTelemetry);
        }

        /// <summary>
        /// Track Event in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="eventTelemetry"></param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <param name="Ip">Ip of device from where request arrived</param>
        /// <returns></returns>
        public static void TrackEvent(EventTelemetry eventTelemetry, string userid = null, string deviceId = null, string Ip = null)
        {
            Telemetry.Context.User.Id = userid ?? string.Empty;
            Telemetry.Context.Device.Id = deviceId ?? string.Empty;
            Telemetry.Context.Location.Ip = Ip ?? string.Empty;
            Telemetry.TrackEvent(eventTelemetry);
        }

        /// <summary>
        /// Track Metric in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="metricTelemetry"></param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <param name="Ip">Ip of device from where request arrived</param>
        /// <returns></returns>
        public static void TrackMetric(MetricTelemetry metricTelemetry, string userid = null, string deviceId = null, string Ip = null)
        {
            Telemetry.Context.User.Id = userid ?? string.Empty;
            Telemetry.Context.Device.Id = deviceId ?? string.Empty;
            Telemetry.Context.Location.Ip = Ip ?? string.Empty;
            Telemetry.TrackMetric(metricTelemetry);
        }

        /// <summary>
        /// Track Dependancy in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="dependencyTelemetry"></param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <param name="Ip">Ip of device from where request arrived</param>
        /// <returns></returns>
        public static void TrackDependancy(DependencyTelemetry dependencyTelemetry, string userid = null, string deviceId = null, string Ip = null)
        {
            Telemetry.Context.User.Id = userid ?? string.Empty;
            Telemetry.Context.Device.Id = deviceId ?? string.Empty;
            Telemetry.Context.Location.Ip = Ip ?? string.Empty;
            Telemetry.TrackDependency(dependencyTelemetry);
        }

        /// <summary>
        /// Track Availability in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="availabilityTelemetry">Availability Telemetry object</param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <param name="Ip">Ip of device from where request arrived</param>
        /// <returns></returns>
        public static void TrackAvailability(AvailabilityTelemetry availabilityTelemetry, string userid = null, string deviceId = null, string Ip = null)
        {
            Telemetry.Context.User.Id = userid ?? string.Empty;
            Telemetry.Context.Device.Id = deviceId ?? string.Empty;
            Telemetry.Context.Location.Ip = Ip ?? string.Empty;
            Telemetry.TrackAvailability(availabilityTelemetry);
        }

        /// <summary>
        /// Track Event in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="pageViewTelemetry">Pageview telemetry object</param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <param name="Ip">Ip of device from where request arrived</param>
        /// <returns></returns>
        public static void TrackPageView(PageViewTelemetry pageViewTelemetry, string userid = null, string deviceId = null, string Ip = null)
        {
            Telemetry.Context.User.Id = userid ?? string.Empty;
            Telemetry.Context.Device.Id = deviceId ?? string.Empty;
            Telemetry.Context.Location.Ip = Ip ?? string.Empty;
            Telemetry.TrackPageView(pageViewTelemetry);
        }
    }
}
