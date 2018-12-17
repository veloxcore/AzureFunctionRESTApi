using MethodDecorator.Fody.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Rest.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rest.Security
{ 
    /// <summary>
    /// CustomAuthorzie Attribute to authorize user before class or method call 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CustomAuthorizeAttribute : Attribute, IAspectMatchingRule
    {
        public string AttributeTargetTypes { get; set; }
        public bool AttributeExclude { get; set; }
        public int AttributePriority { get; set; }
        public int AspectPriority { get; set; }
        
        /// <summary>
        /// HttpRequest to set value on init method and use in other methods
        /// It contains all the details of header and body information so can access details of perticuler request whereever required
        /// </summary>
        HttpRequestMessage _req;
        
        /// <summary>
        /// metadataservice to insert every request header data to database
        /// </summary>
        IMetaDataService _metaDataService;

        /// <summary>
        /// Calls when method or class init 
        /// </summary>
        /// <param name="instance">instance of class or method</param>
        /// <param name="method">methodbase with detail of method</param>
        /// <param name="args">arguements passed in method</param>
        public void Init(object instance, MethodBase method, object[] args)
        {
            foreach (object arg in args)
            {
                if (arg is HttpRequestMessage)
                    _req = (HttpRequestMessage)arg;
                else if (arg is IMetaDataService)
                    _metaDataService = (IMetaDataService)arg;
            }

            if (_req == null)
                throw new ArgumentNullException("Http request message is not injected in the method.");

            if (_metaDataService == null)
                throw new ArgumentNullException("Meta data servic is not injected in the method.");
        }

        /// <summary>
        /// Insert metadata information DeviceId and other key values from header.
        /// Checks the request is authorized or not.
        /// </summary>
        /// <returns>true if not authenticated and false if authenticated</returns>
        public bool NeedBypass()
        {
            _req.AddMetaDataAsync(_metaDataService);
            return !Authentication.ValidateToken(_req.Headers.Authorization);
        }

        /// <summary>
        /// If the request not authorized, then return Unauthorized request.
        /// </summary>
        /// <param name="Retval"></param>
        /// <returns>UnauthorizedResult if request not authorized</returns>
        public async Task<IActionResult> AlterRetval(Task<IActionResult> Retval)
        {
            if(Retval != null)
                return await Retval;

            return await Task.Run<IActionResult>(() => new UnauthorizedResult());
        }
    }
}
