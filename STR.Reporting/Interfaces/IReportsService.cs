using STR.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace STR.Reporting.Interfaces
{
    public interface IReportsService
    {
        Task<(bool IsSuccess, string Error)> ExecuteReportAsync(Guid id);

        Task<(bool IsSuccess, string Error)> UpdateReportStatusAsync(Guid id, EnumReportStatus status);

    }
}
