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

    public class CompanyController(ICompanyService companyService, IUnitOfWork uow,IMapper mapper) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly ICompanyService _companyService = companyService;
        private readonly IMapper _mapper=mapper;   


        [HttpGet]
        [Route("GetCompanies")]
        public async Task<IActionResult> GetCompanies([FromQuery] int page, [FromQuery] int pageSize)
        {
            var query = _uow.Companies.AsQueryable();
            var result = await query.GetPaged(page, pageSize);
            return Ok(result);
        }


        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(AddCompanyViewModel company)
        {
            if (company != null)
            {
                var companyModel = _mapper.Map<Company>(company);
                await _companyService.CompanyAdd(companyModel);
                return Ok("Company added successfully.");   
            }

            else
                return BadRequest("Failed to add company.");
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != null)
            {
                await _companyService.CompanyDelete(id);  
                return Ok("Company deleted successfully.");
            }

            else
                return BadRequest("Failed to delete company.");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Company company)
        {
            if (company != null)
            {
                await _companyService.CompanyUpdate(company);
                return Ok("Company updated successfully.");
            }

            else
                return BadRequest("Failed to update company.");
        }       


    }
}
