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
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyService(IUnitOfWork uow, IMapper mapper, IEmailSenderService emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CompanyAdd(Company company)
        {
            if (company == null)
                throw new DatabaseValidationException("Company is null");

            await _uow.Companies.AddAsync(company);
            return "Company added successfully";
        }

        public async Task<string> CompanyDelete(Guid id)
        {
            if (id == null)
                throw new DatabaseValidationException("Company is null");

            await _uow.Companies.DeleteAsync(id);
            return "Company added successfully";
        }

        public async Task<string> CompanyUpdate(Company company)
        {
            if (company == null)
                throw new DatabaseValidationException("Company is null");

            await _uow.Companies.UpdateAsync(company);
            return "Company updated successfully";
        }


        public async Task<Company> GetCompanyById(Guid companyId)
        {
            if (companyId == null)
                throw new DatabaseValidationException("Company is null");

            return await _uow.Companies.GetByIdAsync(companyId);
        }



    }
}
