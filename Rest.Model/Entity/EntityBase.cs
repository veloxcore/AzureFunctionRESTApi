using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Model.Entity
{
    /// <summary>
    /// Base class for table entity
    /// </summary>
    public abstract class EntityBase
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
