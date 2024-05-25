using AutoMapper;
using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Infrastructure.IRepositories;
using Nowadays.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Nowadays.API.Pagination;
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
            IQueryable<Employee> query = _uow.Employees.AsQueryable();
            PagedViewModel<Employee> result = await query.GetPaged(page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(AddEmployeeViewModel employee)
        {
            if (employee != null)
            {
                Employee employeeModel = _mapper.Map<Employee>(employee);
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
                Employee employeeModel = _mapper.Map<Employee>(employee);
                await _employeeService.EmployeeUpdate(employeeModel);
                return Ok("Employee updated successfully.");
            }
            return BadRequest("Failed to update employee.");
        }

        [HttpPost]
        [Route("BulkAdd")]
        public async Task<IActionResult> BulkAddEmployee(List<AddEmployeeViewModel> employees)
        {
            if (employees == null)
                return BadRequest("employees are not null...");

            List<Employee> employeeList = await _employeeService.BulkEmployeeAdd(employees);
            return Ok(employeeList);
        }
    }
}
