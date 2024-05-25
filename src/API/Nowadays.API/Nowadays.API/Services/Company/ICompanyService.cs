using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nowadays.Entity.Models;

namespace Nowadays.Infrastructure.Services
{
    public interface ICompanyService
    {
        Task<string> CompanyAdd(Company company);
        Task<string> CompanyDelete(Guid id);
        Task<string> CompanyUpdate(Company company);
        Task<Company> GetCompanyById(Guid companyId);
    }
}