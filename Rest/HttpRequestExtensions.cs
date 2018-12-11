using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using Rest.Model.Entity;
using Rest.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rest
{
    /// <summary>
    /// HttpResponseBody
    /// </summary>
    /// <typeparam name="T">Type Parameter</typeparam>
    public class HttpResponseBody<T>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// Isvalid
        /// </value>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// Value
        /// </value>
        public T Value { get; set; }
    }

    /// <summary>
    /// HttpRequest Extenstions
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// GetBody From Request
        /// </summary>
        /// <typeparam name="T">Type of Body value</typeparam>
        /// <param name="request">Httprequest</param>
        /// <returns>Response Body with value</returns>
        public static async Task<HttpResponseBody<T>> GetBodyAsync<T>(this HttpRequestMessage request)
        {
            HttpResponseBody<T> body = new HttpResponseBody<T>();
            var bodyString = await request.Content.ReadAsStringAsync();
            body.Value = JsonConvert.DeserializeObject<T>(bodyString);
            return body;
        }

        /// <summary>
        /// To Add Metadata from request header
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <param name="metaDataService">Metadata service</param>
        /// <returns></returns>
        public static async Task AddMetaDataAsync(this HttpRequestMessage request, IMetaDataService metaDataService)
        {
            // Dictionary<string, string> headerBlocks = new Dictionary<string, string>();
            MetaData metadata = new MetaData();
            metadata.TimeStamp = DateTime.UtcNow;
            var deviceID = request.Headers.Contains("DeviceId") ? request.Headers.GetValues("DeviceId") : null;
            if (deviceID != null)
            {
                deviceID = deviceID.ToList();
                metadata.SetValue("DeviceId", deviceID.FirstOrDefault());
            }
            var payloadPairs = new Dictionary<string, string>();
            foreach (string key in PayLoadKeys)
            {
                var value = request.Headers.Contains(key) ? request.Headers.GetValues(key) : null;
                if (value != null)
                {
                    value = value.ToList();
                    payloadPairs.Add(key, value.FirstOrDefault());
                }
            }
            if (payloadPairs.Count > 0)
               metadata.SetValue("Payload",JsonConvert.SerializeObject(payloadPairs));
            if (!string.IsNullOrEmpty(metadata.DeviceId))
               await metaDataService.InsertMetaDataAsync(metadata);
        }

        /// <summary>
        /// Setvalue value in metadata object
        /// </summary>
        /// <param name="metadata">Metadata object</param>
        /// <param name="key">Key</param>
        /// <param name="value">value to assign</param>
        /// <returns>object with assigned value</returns>
        private static MetaData SetValue(this MetaData metadata, string key, string value)
        {
            switch (key)
            {
                case "DeviceId":
                    metadata.DeviceId = value;
                    break;
                case "Payload":
                    metadata.Payload = value;
                    break;
            }
            return metadata;
        }

        /// <summary>
        /// Keys which are stored in json as Headers with comman separted.
        /// This insert keys assigned in startup from configuration.
        /// Header values with this keys will be inserted in json format in metadata table.
        /// </summary>
        public static List<string> PayLoadKeys = new List<string>();
    }
}
