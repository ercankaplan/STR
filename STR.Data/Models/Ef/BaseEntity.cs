using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Data.Models.Ef
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime? UpdateTime { get; set; }
    }
}
