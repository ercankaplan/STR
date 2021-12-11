using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.Report.Models
{
    public class ReportRequest
    {
        public Guid Id { get; set; }

        public Guid ReportId { get; set; }

        public byte Status { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime? UpdateTime { get; set; }
    }
}
