﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STR.Api.Report.Interfaces;
using STR.Api.Report.Models;
using STR.Data.Models;
using STR.Data.Models.Ef.EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.Report.Providers
{
    public class ReportsProvider : IReportsProvider
    {

        private readonly STRDbContext dbContext;
        private readonly ILogger<ReportsProvider> logger;
        private readonly IMapper mapper;
        public ReportsProvider(STRDbContext dbContext, ILogger<ReportsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;


        }
        public async Task<(bool IsSuccess, string Error)> AddReportRequestAsync(ReportRequest model)
        {
            try
            {
                Data.Models.Ef.ReportRequest reportRequest = mapper.Map<ReportRequest, Data.Models.Ef.ReportRequest>(model);

                reportRequest.CreatedTime = DateTime.Now;
                reportRequest.Id = Guid.NewGuid();
                reportRequest.Status = (byte)EnumReportStatus.Pending;

                dbContext.ReportRequest.Add(reportRequest);
                await dbContext.SaveChangesAsync();

                return (true, "Added Report");
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ReportRequest ReportRequest, string Error)> GetReportRequestAsync(Guid id)
        {
            try
            {
                var reportRequest = await dbContext.ReportRequest.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (reportRequest != null)
                {
                    logger?.LogInformation("ReportRequest Found");

                    ReportRequest result = mapper.Map<Data.Models.Ef.ReportRequest, ReportRequest>(reportRequest);
                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ReportRequest> ReportRequests, string Error)> GetReportRequestsAsync()
        {
            try
            {
                var reportRequests = await dbContext.ReportRequest.ToListAsync();

                if (reportRequests.Any())
                {
                    logger?.LogInformation("ReportRequests Found");

                    var result = mapper.Map<List<Data.Models.Ef.ReportRequest>, List<ReportRequest>>(reportRequests);

                    return (true, result, null);

                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.StackTrace);
                return (false, null, ex.Message);
            }
        }
    }
}
