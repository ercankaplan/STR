using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.Report.Models
{
    public class ReportResult
    {
        public Guid Id { get; set; }

        public Guid ReportRequestId { get; set; }

        public string Result { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime? UpdateTime { get; set; }
    }
}
