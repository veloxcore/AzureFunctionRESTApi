using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Rest.Data.Entity
{
    [Table("Book")]
    public class Book : EntityBase
    {
        [StringLength(40)]
        public string BookId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public int NumberOfPages { get; set; }

        public DateTime DateOfPublication { get; set; }

        [StringLength(500)]
        public string Authors { get; set; }
    }
}
