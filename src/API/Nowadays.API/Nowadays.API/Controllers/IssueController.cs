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
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public IssueController(IIssueService issueService, IUnitOfWork uow, IMapper mapper)
        {
            _issueService = issueService;
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetIssues")]
        public async Task<IActionResult> GetIssues([FromQuery] int page, [FromQuery] int pageSize)
        {
            var query = _uow.Issue.AsQueryable();
            var result = await query.GetPaged(page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(AddIssueViewModel issue)
        {
            if (issue != null)
            {
                var issueModel = _mapper.Map<Issue>(issue);
                await _issueService.IssueAdd(issueModel);
                return Ok("Issue added successfully.");
            }

            return BadRequest("Failed to add issue.");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Issue issue)
        {
            if (issue != null)
            {
                await _issueService.IssueUpdate(issue);
                return Ok("Issue updated successfully.");
            }
            else
                return BadRequest("Failed to update Issue.");
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != null)
            {
                await _issueService.IssueDelete(id);
                return Ok("Issue deleted successfully.");   
            }

            return BadRequest("Failed to add issue.");
        }
    }
}
