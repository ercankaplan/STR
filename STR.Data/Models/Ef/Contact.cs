using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Data.Models.Ef
{
    public class Contact : BaseEntity
    {
        public Guid PersonId { get; set; }

        public byte ContactType { get; set; }

        public string ContactInfo { get; set; }

  
        public virtual Person Person { get; set; }
    }
}
