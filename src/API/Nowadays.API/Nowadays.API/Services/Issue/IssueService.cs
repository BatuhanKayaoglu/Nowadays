using AutoMapper;
using Nowadays.Entity.Models;
using Nowadays.Infrastructure.IRepositories;
using Nowadays.API.Exceptions;
using System.Collections.Generic;
using System.Text;
using Nowadays.API.Services.EmailSender;

namespace Nowadays.Infrastructure.Services
{
    public class IssueService : IIssueService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IssueService(IUnitOfWork uow, IMapper mapper, IEmailSenderService emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> IssueAdd(Issue issue)
        {
            if (issue == null)
                throw new DatabaseValidationException("Issue is null");

            await _uow.Issue.AddAsync(issue);
            return "Employee added successfully";
        }

        public async Task<string> IssueDelete(Guid id)
        {
            if (id == null)
                throw new DatabaseValidationException("Issue is null");

            await _uow.Issue.DeleteAsync(id);
            return "Issue added successfully";
        }

        public async Task<string> IssueUpdate(Issue issue)
        {
            if (issue == null)
                throw new DatabaseValidationException("Issue is null");

            await _uow.Issue.UpdateAsync(issue);
            return "Issue updated successfully";
        }

        public async Task<Employee> AddEmployeeToIssue(Guid IssueId, Guid employeeId)
        {
            Issue issue = await _uow.Issue.GetByIdAsync(IssueId);
            Employee employee = await _uow.Employees.GetByIdAsync(employeeId);

            if (employee is null)
                throw new DatabaseValidationException("employee is null");

            if (issue is null)
                throw new DatabaseValidationException("issue is null");

            issue.Assignees.Add(employee);

            return employee;
        }

        public async Task<List<Guid>> MultipleAddEmployeeToIssue(List<Guid> employeeIds, Guid IssueId)
        {
            List<Guid> addedEmployees = new List<Guid>();

            foreach(var employeeId in employeeIds)
            {
                addedEmployees.Add(employeeId);
            }

            Issue issue = await _uow.Issue.GetByIdAsync(IssueId);   
            foreach (var employee in addedEmployees){
                Employee employeeToAdd = await _uow.Employees.GetByIdAsync(employee);
                issue.Assignees.Add(employeeToAdd);
            }   
            return addedEmployees;  



        }
    }
}
