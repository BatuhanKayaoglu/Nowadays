using AutoMapper;
using Azure.Core;
using Nowadays.Common.Extensions;
using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models;
using Nowadays.Entity.Models.Identity;
using Nowadays.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nowadays.API.Exceptions;
using Nowadays.API.Extensions.JwtConf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Nowadays.API.Services.EmailSender;

namespace Nowadays.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(IUnitOfWork uow, IMapper mapper, IEmailSenderService emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> EmployeeAdd(Employee employee)
        {
            if (employee == null)
                throw new DatabaseValidationException("Employee is null");

            await _uow.Employees.AddAsync(employee);
            return "Employee added successfully";
        }

        public async Task<string> EmployeeDelete(Guid id)
        {
            if (id == null)
                throw new DatabaseValidationException("Employee is null");

            await _uow.Employees.DeleteAsync(id);
            return "Employee added successfully";
        }

        public async Task<string> EmployeeUpdate(Employee employee)
        {
            if (employee == null)
                throw new DatabaseValidationException("Employee is null");

            await _uow.Employees.UpdateAsync(employee);
            return "Employee updated successfully";
        }




    }
}
