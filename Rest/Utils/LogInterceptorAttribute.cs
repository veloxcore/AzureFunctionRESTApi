using MethodDecorator.Fody.Interfaces;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Rest.Utils
{
    /// <summary>
    /// LogInterceptor to log on method's init, entry ,exit or exceptions points
    /// Put as [LogInterceptor] on top of class to apply on all the methods or method to apply on specific method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class LogInterceptorAttribute : Attribute, IMethodDecorator
    {
        /// <summary>
        /// MethodBase contains detail of particular method 
        /// Set it's value on init method to use in other methods of LogInterCeptorAttribute
        /// </summary>
        MethodBase _method;
       
        /// <summary>
        /// Call on init of any method where attribute used
        /// </summary>
        /// <param name="instance">instance</param>
        /// <param name="method">methodbase detail of method</param> 
        /// <param name="args">arguements supplied in method</param>
        public void Init(object instance, MethodBase method, object[] args)
        {
            //Set value to _method object of this class to use in other methods of this class
            _method = method;
        }

        /// <summary>
        /// Calls on method entry 
        /// </summary>
        public void OnEntry()
        {
        }

        /// <summary>
        /// Calls When exception occures in method execution
        /// </summary>
        public void OnException(Exception exception)
        {
            //ExceptionTelemetry object of application insights

            var trace = new ExceptionTelemetry() { Message = exception.Message, Timestamp = DateTime.UtcNow, SeverityLevel = SeverityLevel.Critical };
            trace.Exception = exception;

            //These are customDimensions of application insight's table to find logs by below properties in query
            //Sample query to run in application insights: exceptions | extend sortKey = tostring(customDimensions.MethodName) | where sortKey=='Value'
            //Replace Value by your property value in query
            
            //Add Properties key value pair
            trace.Properties.Add("MethodName", _method.Name);
            trace.Properties.Add("DeclaringType", _method.DeclaringType.Name);
            Logger.TrackException(trace);
        }

        /// <summary>
        /// call on exit point of method
        /// </summary>
        public void OnExit()
        {
        }
    }
}
