using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Data.Models.Ef
{
    public class ReportResult :BaseEntity
    {
        public Guid ReportRequestId { get; set; }

        public string Result { get; set; }

        public virtual ReportRequest ReportRequest { get; set; }
    }
}
