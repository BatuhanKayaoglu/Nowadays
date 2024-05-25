using AutoMapper;
using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Infrastructure.IRepositories;
using Nowadays.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nowadays.API.Exceptions;
using Nowadays.API.Extensions.JwtConf;
using Nowadays.API.Pagination;
using Nowadays.API.Services;
using System.ComponentModel.DataAnnotations;
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
            var query = _uow.Report.AsQueryable();
            var result = await query.GetPaged(page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(Report report)
        {
            if (report != null)
            {
                await _reportService.EmployeeAdd(employeeModel);
                return Ok("Employee added successfully.");
            }

            return BadRequest("Failed to add employee.");
        }


    }
}
