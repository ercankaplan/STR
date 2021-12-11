using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Data.Models.Ef
{
    public class ReportRequest : BaseEntity
    {
        
        public Guid ReportId { get; set; }
        public byte Status { get; set; }

        public virtual Report Report { get; set; }

        public virtual ReportResult ReportResult { get; set; }
    }
}
