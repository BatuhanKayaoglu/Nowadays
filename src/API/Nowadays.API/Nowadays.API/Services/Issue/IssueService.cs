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




    }
}
