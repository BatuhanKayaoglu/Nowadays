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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IUnitOfWork uow, IMapper mapper)
        {
            _projectService = projectService;
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetProjects")]
        public async Task<IActionResult> GetProjects([FromQuery] int page, [FromQuery] int pageSize)
        {
            var query = _uow.Projects.AsQueryable();
            var result = await query.GetPaged(page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(AddProjectViewModel project)
        {
            if (project != null)
            {
                var projectModel = _mapper.Map<Project>(project);
                await _projectService.ProjectAdd(projectModel);
                return Ok("Project added successfully.");
            }

            return BadRequest("Failed to add project.");
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != null)
            {
                await _projectService.ProjectDelete(id);
                return Ok("Project deleted successfully.");
            }
            else
                return BadRequest("Failed to delete project.");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Project project)
        {
            if (project != null)
            {
                await _projectService.ProjectUpdate(project);
                return Ok("Project updated successfully.");
            }
            else
                return BadRequest("Failed to update project.");
        }

        [HttpPost]
        [Route("AssignTask")]
        public async Task<IActionResult> AssignTask(Guid projectId, Guid assigneeId)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid project ID or assignee ID.");
            }
            var project = await _uow.Projects.GetByIdAsync(projectId);
            if (project == null)
            {
                return NotFound("Project not found.");
            }

            var assignee = await _uow.Employees.GetByIdAsync(assigneeId);
            if (assignee == null)
            {
                return NotFound("Assignee not found.");
            }
            await _projectService.ProjectUpdate(project);

            return Ok("Task assigned successfully.");
        }
    }

}
