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
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CustomAuthorizeAttribute : Attribute, IAspectMatchingRule
    {
        public string AttributeTargetTypes { get; set; }
        public bool AttributeExclude { get; set; }
        public int AttributePriority { get; set; }
        public int AspectPriority { get; set; }
        
        HttpRequestMessage _req;
        IMetaDataService _metaDataService;

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

        public bool NeedBypass()
        {
            _req.AddMetaDataAsync(_metaDataService);
            return !Authentication.ValidateToken(_req.Headers.Authorization);
        }

        public async Task<IActionResult> AlterRetval(Task<IActionResult> Retval)
        {
            if(Retval != null)
                return await Retval;

            return await Task.Run<IActionResult>(() => new UnauthorizedResult());
        }
    }
}
