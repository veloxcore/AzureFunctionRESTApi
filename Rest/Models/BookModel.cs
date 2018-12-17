using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    /// <summary>
    /// UI Data model for Book
    /// </summary>
    public class BookModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name with max length 250
        /// </value>
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the number of pages.
        /// </summary>
        /// <value>
        /// The number of pages.
        /// </value>
        public int NumberOfPages { get; set; }
        /// <summary>
        /// Gets or sets the date of publication.
        /// </summary>
        /// <value>
        /// The date of publication.
        /// </value>
        public DateTime DateOfPublication { get; set; }
        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>
        /// The authors array.
        /// </value>
        public string[] Authors { get; set; }
    }
    
}

