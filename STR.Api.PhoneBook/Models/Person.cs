using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.PhoneBook.Models
{
    public class Person
    {
        public Guid Id { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime? UpdateTime { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string CompanyName { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}
