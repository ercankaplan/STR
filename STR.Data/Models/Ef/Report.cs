using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Data.Models.Ef
{
    public class Report : BaseEntity
    {
        public string Description { get; set; }
        public string Name { get; set; }

        public ICollection<ReportRequest> ReportRequests { get; set; }

    }
}
