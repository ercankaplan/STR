using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Data.Models.Ef
{
    public class Person : BaseEntity
    {
    
        public string Name { get; set; }

        public string Surname { get; set; }

        public string CompanyName { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}
