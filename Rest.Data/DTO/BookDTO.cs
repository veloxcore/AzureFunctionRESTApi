using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Data.DTO
{
    public class BookDTO
    {
        public string BookId { get; set; }
        public string Name { get; set; }
        public int NumberOfPages { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string[] Authors { get; set; }
    }
}
