using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Models
{
    /// <summary>
    /// Error response class for sending erroneous response to client.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        public Error()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="msg">The MSG.</param>
        public Error(string code, string msg)
        {
            this.Code = code;
            this.Message = msg;
        }
    }
}
