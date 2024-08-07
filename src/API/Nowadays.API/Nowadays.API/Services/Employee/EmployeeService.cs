﻿using AutoMapper;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models;
using Nowadays.Infrastructure.IRepositories;
using Nowadays.API.Exceptions;
using System.Collections.Generic;
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
                throw new UserCreateFailedException("Employee not created!");

            await _uow.Employees.AddAsync(employee);
            return "Employee created successfully";
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

        public async Task<Employee> GetEmployeeById(Guid id)
        {
            if (id == null)
                throw new DatabaseValidationException("Employee is null");

            return await _uow.Employees.GetByIdAsync(id);
        }

        public async Task<List<Employee>> BulkEmployeeAdd(List<AddEmployeeViewModel> employees)
        {
            if (employees == null)
                throw new DatabaseValidationException("Employee is null");

            List<Employee> employeeList = _mapper.Map<List<Employee>>(employees);
            await _uow.Employees.BulkAdd(employeeList);

            return employeeList;
        }


    }
}
