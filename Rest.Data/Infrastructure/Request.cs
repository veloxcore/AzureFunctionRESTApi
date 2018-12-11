using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Rest.Data.Infrastructure
{
    /// <summary>
    /// Database request helper, containing command (query) and parameters
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Gets or sets the request parameters.
        /// </summary>
        /// <value>
        /// The request parameters.
        /// </value>
        List<SqlParameter> RequestParameters { get; set; }

        /// <summary>
        /// Gets or sets the commands.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        string Commands { get; set; }
    }

    /// <summary>
    /// Database request helper, containing command (query) and parameters
    /// </summary>
    public class Request : IRequest
    {
        #region Members

        private string _commands;
        private List<SqlParameter> _requestParameters;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the commands.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        public string Commands
        {
            get { return this._commands; }
            set { this._commands = value; }
        }

        /// <summary>
        /// Gets or sets the request parameters.
        /// </summary>
        /// <value>
        /// The request parameters.
        /// </value>
        public List<SqlParameter> RequestParameters
        {
            get { return this._requestParameters; }
            set { this._requestParameters = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Request" /> class.
        /// </summary>
        public Request()
        {
            this.Commands = "";
            this.RequestParameters = new List<SqlParameter>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Request" /> class.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <param name="requestSource">The request source.</param>
        /// <param name="requestParameters">The request parameters.</param>
        public Request(string commands, string requestSource, List<SqlParameter> requestParameters)
        {
            this.Commands = commands;
            this.RequestParameters = requestParameters;
        }

        #endregion Constructors

        #region Override Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string str = string.Empty;
            str += "Commands:" + this.Commands + Environment.NewLine;
            if (RequestParameters != null)
            {
                str += "RequestParametersCount: " + RequestParameters.Count;
            }

            return str;
        }

        #endregion Override Methods
    }
}
