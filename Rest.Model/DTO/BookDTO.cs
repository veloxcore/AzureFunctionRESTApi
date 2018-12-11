using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Model.DTO
{
    /// <summary>
    /// BookDTO Class to map book entity to BookDTO and return in api reponses
    /// </summary>
    public class BookDTO
    {
        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Id of book
        /// </value>
        public string BookId { get; set; }

        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Name of book
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Number of pages of book
        /// </value>
        public int NumberOfPages { get; set; }

        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Date of publication
        /// </value>
        public DateTime DateOfPublication { get; set; }

        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Author's names of book
        /// </value>
        public string[] Authors { get; set; }
    }
}
