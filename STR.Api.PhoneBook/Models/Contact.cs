using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.PhoneBook.Models
{
    public class Contact
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }

        public byte ContactType { get; set; }

        public string ContactInfo { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime? UpdateTime { get; set; }
    }
}
