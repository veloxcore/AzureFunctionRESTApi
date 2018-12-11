using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Rest.Model.Entity
{
    /// <summary>
    /// Book Entity of Database Table
    /// </summary>
    [Table("Book")]
    public class Book : EntityBase
    {
        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// BookId
        /// </value>
        [StringLength(40)]
        public string BookId { get; set; }

        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Name of book
        /// </value>
        [StringLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Get or sets value
        /// </summary>
        /// <value>
        /// Name of pages
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
        /// Author's name of book
        /// </value>
        [StringLength(500)]
        public string Authors { get; set; }
    }
}
