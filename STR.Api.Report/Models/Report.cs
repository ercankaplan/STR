using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.Report.Models
{
    public class Report
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime? UpdateTime { get; set; }
    }
}
