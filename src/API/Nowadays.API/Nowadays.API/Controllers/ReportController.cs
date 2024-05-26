using AutoMapper;
using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Infrastructure.IRepositories;
using Nowadays.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Nowadays.API.Exceptions;
using Nowadays.API.Pagination;
using Nowadays.API.Services;
using System.Text;
using Nowadays.Entity.Models;

namespace Nowadays.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ReportController(IReportService reportService, IUnitOfWork uow, IMapper mapper)
        {
            _reportService = reportService;
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetReports")]
        public async Task<IActionResult> GetReports([FromQuery] int page, [FromQuery] int pageSize)
        {
            IQueryable<Report> query = _uow.Report.AsQueryable();
           PagedViewModel<Report> result = await query.GetPaged(page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(Report report)
        {
            if (report != null)
            {
                await _reportService.ReportAdd(report);
                return Ok("Report added successfully.");
            }

            return BadRequest("Failed to add report.");
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != null)
            {
                await _reportService.ReportDelete(id);
                return Ok("Report deleted successfully.");
            }

            else
                return BadRequest("Failed to delete Report.");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Report report)
        {
            if (report != null)
            {
                await _reportService.ReportUpdate(report);
                return Ok("Report updated successfully.");
            }

            else
                return BadRequest("Failed to update company.");
        }


    }
}
