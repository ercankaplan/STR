using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using STR.Data.Models;
using STR.Data.Models.Ef;
using STR.Data.Models.Ef.EfContext;
using STR.Reporting.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR.Reporting.Services
{
    public class ReportsService : IReportsService
    {

        private readonly STRDbContext dbContext;
  


        public ReportsService(STRDbContext dbContext)
        {
            this.dbContext = dbContext;
  

        }

        public async Task<(bool IsSuccess, string Error)> ExecuteReportAsync(Guid id)
        {
            try
            {

                ReportRequest reportRequest =  dbContext.ReportRequest.Where(o => o.Id == id).SingleOrDefault();

                //TODO execute sp get result 

                Report report = dbContext.Report.Where(x => x.Id == reportRequest.ReportId).SingleOrDefault();

                string result = "Dummy Result";

                if (report != null)
                {
                    if (report.Name == "PERSON_COUNT")
                    {
                        result = $"PERSON_COUNT:{dbContext.Person.Count()}";
                    }
                    else if (report.Name == "PHONE_NUMBER_COUNT")
                    {
                        result = $"PHONE_NUMBER_COUNT:{dbContext.Contact.Where(o=> o.ContactType == (byte)EnumContactType.Phone).Count()}";
                    }
                }


                ReportResult reportResult = new ReportResult()
                {
                    Id = Guid.NewGuid(),
                    ReportRequestId = reportRequest.Id,
                    Result = result,
                    CreatedTime = DateTime.Now
                };

                dbContext.ReportResult.Add(reportResult);

                reportRequest.Status = (byte)EnumReportStatus.Ready;

                //Task.Delay(1000);


                await dbContext.SaveChangesAsync();


                return (true, "Executed Report");
            }
            catch (Exception ex)
            {

           

                await this.UpdateReportStatusAsync(id, EnumReportStatus.Fail);

                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string Error)> UpdateReportStatusAsync(Guid id, EnumReportStatus newStatus)
        {
            try
            {

                ReportRequest reportRequest =  dbContext.ReportRequest.Where(o => o.Id == id).SingleOrDefault();

                if (reportRequest == null)
                    throw new ApplicationException("Not Found ReportRequest");

                reportRequest.Status = (byte)newStatus;
                reportRequest.UpdateTime = DateTime.Now;

                await dbContext.SaveChangesAsync();

                return (true, "Updated Report Status");
            }
            catch (Exception ex)
            {

         
                return (false, ex.Message);
            }
        }

       

    }
}
