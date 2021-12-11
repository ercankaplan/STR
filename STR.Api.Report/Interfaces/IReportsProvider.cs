using STR.Api.Report.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.Report.Interfaces
{
    public interface IReportsProvider
    {
      
        /// <summary>
        /// Rehberdeki kişilerin bulundukları konuma göre istatistiklerini çıkartan bir rapor talebi
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task<(bool IsSuccess, string Error)> AddReportRequestAsync(ReportRequest model);

        /// <summary>
        /// Sistemin oluşturduğu raporların listelenmesi
        /// </summary>
        /// <returns></returns>
        Task<(bool IsSuccess, IEnumerable<ReportRequest> ReportRequests, string Error)> GetReportRequestsAsync();

        /// <summary>
        /// Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi
        /// </summary>
        /// <returns></returns>
        Task<(bool IsSuccess, ReportRequest ReportRequest, string Error)> GetReportRequestAsync(Guid id);
    }
}
