using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Data.Entity
{
    public abstract class EntityBase
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
