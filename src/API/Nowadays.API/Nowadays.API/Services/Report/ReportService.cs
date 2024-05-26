using AutoMapper;
using Nowadays.Entity.Models;
using Nowadays.Infrastructure.IRepositories;
using Nowadays.API.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nowadays.API.Services.EmailSender;

namespace Nowadays.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(IUnitOfWork uow, IMapper mapper, IEmailSenderService emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> ReportAdd(Report report)
        {
            if (report == null)
                throw new DatabaseValidationException("Report is null");

            await _uow.Report.AddAsync(report);
            return "Report added successfully";
        }

        public async Task<string> ReportDelete(Guid id)
        {
            if (id == null)
                throw new DatabaseValidationException("id is null");

            await _uow.Report.DeleteAsync(id);
            return "Report added successfully";
        }

        public async Task<string> ReportUpdate(Report report)
        {
            if (report == null)
                throw new DatabaseValidationException("Report is null");

            await _uow.Report.UpdateAsync(report);
            return "Report updated successfully";
        }


    }
}
