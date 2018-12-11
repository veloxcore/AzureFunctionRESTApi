using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Rest.Model.Entity
{
    /// <summary>
    /// Book Entity of Database Table
    /// </summary>
    [Table("MetaData")]
    public class MetaData
    {
        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Id of book
        /// </value>
        public long Id{ get; set; }

        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// DeviceId
        /// </value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Timestamp
        /// </value>
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Payload - Information from header with keys that are exist in local settings json file named PayloadKeys
        /// </value>
        public string Payload { get; set; }

    }
}
