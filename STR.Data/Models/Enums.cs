using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Data.Models
{
    public enum EnumContactType
    {
        Phone,
        Email,
        Location
    }

    public enum EnumReportStatus
    { 
        Pending,
        Processing,
        Ready,
        Fail
    }
}
