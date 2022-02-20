using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STR.Api.Report.Interfaces;
using STR.Api.Report.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsProvider mReportsProvider;

        public ReportsController(IReportsProvider reportProvider)
        {
            mReportsProvider = reportProvider;
        }

        [HttpPost]
        public async Task<IActionResult> AddReportRequestAsync(ReportRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model");

            var result = await mReportsProvider.AddReportRequestAsync(model);

            return Ok(result.IsSuccess);

        }

        [HttpGet]
        public async Task<IActionResult> GetReportRequestsAsync()
        {
            var result = await mReportsProvider.GetReportRequestsAsync();

            if (result.IsSuccess)
                return Ok(result.ReportRequests);

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportRequestAsync(Guid id)
        {
            var result = await mReportsProvider.GetReportRequestAsync(id);

            if (result.IsSuccess)
                return Ok(result.ReportRequest);

            return NotFound();
        }
    }
}
