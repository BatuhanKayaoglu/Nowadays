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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IUnitOfWork uow, IMapper mapper)
        {
            _employeeService = employeeService;
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetEmployees([FromQuery] int page, [FromQuery] int pageSize)
        {
            var query = _uow.Employees.AsQueryable();
            var result = await query.GetPaged(page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(AddEmployeeViewModel employee)
        {
            if (employee != null)
            {
                var employeeModel = _mapper.Map<Employee>(employee);
                await _employeeService.EmployeeAdd(employeeModel);
                return Ok("Employee added successfully.");
            }

            return BadRequest("Failed to add employee.");
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != null)
            {
                await _employeeService.EmployeeDelete(id);
                return Ok("Employee deleted successfully.");
            }

            return BadRequest("Failed to delete employee.");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(UpdateEmployeeViewModel employee)
        {
            if (employee != null)
            {
                var employeeModel = _mapper.Map<Employee>(employee);
                await _employeeService.EmployeeUpdate(employeeModel);
                return Ok("Employee updated successfully.");
            }

            return BadRequest("Failed to update employee.");
        }       
    }
}
