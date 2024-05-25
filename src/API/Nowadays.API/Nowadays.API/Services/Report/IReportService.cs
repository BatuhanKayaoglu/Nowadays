using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nowadays.Entity.Models;

namespace Nowadays.Infrastructure.Services
{
    public interface IReportService
    {
        Task<string> ReportAdd(Report report);
        Task<string> ReportDelete(Guid id);
        Task<string> ReportUpdate(Report report);
    }
}