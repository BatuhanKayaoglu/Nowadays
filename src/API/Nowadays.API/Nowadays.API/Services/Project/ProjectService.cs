using AutoMapper;
using Nowadays.Entity.Models;
using Nowadays.Infrastructure.IRepositories;
using Nowadays.API.Exceptions;
using System.Collections.Generic;
using System.Text;
using Nowadays.API.Services.EmailSender;
using System.Linq.Expressions;

namespace Nowadays.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectService(IUnitOfWork uow, IMapper mapper, IEmailSenderService emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> ProjectAdd(Project project)
        {
            if (project == null)
                throw new DatabaseValidationException("Project is null");

            await _uow.Projects.AddAsync(project);
            await _emailSender.SendEmailAsync("softwareworkacc@gmail.com", "adding a project", "A new project has been added");
            return "Project added successfully";
        }

        public async Task<string> ProjectDelete(Guid id)
        {
            if (id == null)
                throw new DatabaseValidationException("Project is null");

            await _uow.Projects.DeleteAsync(id);
            return "Project added successfully";
        }

        public async Task<string> ProjectUpdate(Project project)
        {
            if (project == null)
                throw new DatabaseValidationException("Company is null");

            await _uow.Projects.UpdateAsync(project);
            return "project updated successfully";
        }

        public async Task<Project> GetProjectById(Guid id)
        {
            if (id == null)
                throw new DatabaseValidationException("Project is null");

            return await _uow.Projects.GetByIdAsync(id);
        }


        public async Task<string> BulkDeleteProject(List<Guid> projectIds)
        {
            Expression<Func<Project, bool>> predicate = project => projectIds.Contains(project.Id);
            await _uow.Projects.BulkDelete(predicate);

            return "Projects deleted successfully";     
        }



    }
}
