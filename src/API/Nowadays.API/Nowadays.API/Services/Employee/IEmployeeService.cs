﻿using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nowadays.Entity.Models;

namespace Nowadays.Infrastructure.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> BulkEmployeeAdd(List<AddEmployeeViewModel> employees);
        Task<string> EmployeeAdd(Employee employee);
        Task<string> EmployeeDelete(Guid id);
        Task<string> EmployeeUpdate(Employee employee);
        Task<Employee> GetEmployeeById(Guid id);
    }
}