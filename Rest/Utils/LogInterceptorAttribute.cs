using MethodDecorator.Fody.Interfaces;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rest.Utils
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class LogInterceptorAttribute : Attribute, IMethodDecorator
    {
        public void Init(object instance, MethodBase method, object[] args)
        {
        }

        public void OnEntry()
        {
        }

        public void OnException(Exception exception)
        {
            var trace = new ExceptionTelemetry() { Message = exception.Message, Timestamp = DateTime.UtcNow, SeverityLevel = SeverityLevel.Critical };
            trace.Exception = exception;
            Logger.TrackException(trace);
        }

        public void OnExit()
        {
        }
    }
}
