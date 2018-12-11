
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rest
{
    /// <summary>
    /// ILogger extension methods for logs into application insights of different categories
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Telementry client to track different categories in application insights
        /// </summary>
        public static readonly TelemetryClient telemetry = new TelemetryClient();

        /// <summary>
        /// Track Request in application insights
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="requestTelemetry">Request Telemtry object</param>
        /// <param name="userid">user id </param>
        /// <param name="deviceId">device id</param>
        /// <returns></returns>
        public static async Task TrackRequestAsync(this ILogger logger, RequestTelemetry requestTelemetry,string userid=null,string deviceId=null)
        {
            telemetry.Context.User.Id = userid ?? string.Empty;
            telemetry.Context.Device.Id = deviceId ?? string.Empty;
            telemetry.Context.Location.Ip= deviceId??string.Empty;
            telemetry.TrackRequest(requestTelemetry);

        }

        /// <summary>
        /// Track Exception in application insights 
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="exceptionTelemetry">Exception Telemetry</param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <returns></returns>
        public static async Task TrackExceptionAsync(this ILogger logger, ExceptionTelemetry exceptionTelemetry, string userid = null, string deviceId = null)
        {
            telemetry.Context.User.Id = userid ?? string.Empty;
            telemetry.Context.Device.Id = deviceId ?? string.Empty;
            telemetry.Context.Location.Ip = deviceId ?? string.Empty;
            telemetry.TrackException(exceptionTelemetry);
        }

        /// <summary>
        /// Track Trace in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="traceTelemetry">Tracetelemetry object</param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <returns></returns>
        public static async Task TrackTraceAsync(this ILogger logger, TraceTelemetry traceTelemetry, string userid = null, string deviceId = null)
        {
            telemetry.Context.User.Id = userid ?? string.Empty;
            telemetry.Context.Device.Id = deviceId ?? string.Empty;
            telemetry.Context.Location.Ip = deviceId ?? string.Empty;
            telemetry.TrackTrace(traceTelemetry);
        }

        /// <summary>
        /// Track Event in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="eventTelemetry"></param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <returns></returns>
        public static async Task TrackEventAsync(this ILogger logger, EventTelemetry eventTelemetry, string userid = null, string deviceId = null)
        {
            telemetry.Context.User.Id = userid ?? string.Empty;
            telemetry.Context.Device.Id = deviceId ?? string.Empty;
            telemetry.Context.Location.Ip = deviceId ?? string.Empty;
            telemetry.TrackEvent(eventTelemetry);
        }

        /// <summary>
        /// Track Metric in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="metricTelemetry"></param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <returns></returns>
        public static async Task TrackMetricAsync(this ILogger logger, MetricTelemetry metricTelemetry, string userid = null, string deviceId = null)
        {
            telemetry.Context.User.Id = userid ?? string.Empty;
            telemetry.Context.Device.Id = deviceId ?? string.Empty;
            telemetry.Context.Location.Ip = deviceId ?? string.Empty;
            telemetry.TrackMetric(metricTelemetry);
        }

        /// <summary>
        /// Track Dependancy in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="dependencyTelemetry"></param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <returns></returns>
        public static async Task TrackDependancyAsync(this ILogger logger, DependencyTelemetry dependencyTelemetry, string userid = null, string deviceId = null)
        {
            telemetry.Context.User.Id = userid ?? string.Empty;
            telemetry.Context.Device.Id = deviceId ?? string.Empty;
            telemetry.Context.Location.Ip = deviceId ?? string.Empty;
            telemetry.TrackDependency(dependencyTelemetry);
        }

        /// <summary>
        /// Track Availability in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="availabilityTelemetry">Availability Telemetry object</param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <returns></returns>
        public static async Task TrackAvailabilityAsync(this ILogger logger, AvailabilityTelemetry availabilityTelemetry, string userid = null, string deviceId = null)
        {
            telemetry.Context.User.Id = userid ?? string.Empty;
            telemetry.Context.Device.Id = deviceId ?? string.Empty;
            telemetry.Context.Location.Ip = deviceId ?? string.Empty;
            telemetry.TrackAvailability(availabilityTelemetry);
        }

        /// <summary>
        /// Track Event in application insights
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="pageViewTelemetry">Pageview telemetry object</param>
        /// <param name="userid">user id</param>
        /// <param name="deviceId">device id</param>
        /// <returns></returns>
        public static async Task TrackPageViewAsync(this ILogger logger, PageViewTelemetry pageViewTelemetry, string userid = null, string deviceId = null)
        {
            telemetry.Context.User.Id = userid ?? string.Empty;
            telemetry.Context.Device.Id = deviceId ?? string.Empty;
            telemetry.Context.Location.Ip = deviceId ?? string.Empty;
            telemetry.TrackPageView(pageViewTelemetry);
        }

    }
}
