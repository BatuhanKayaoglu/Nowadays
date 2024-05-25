using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nowadays.Entity.Models;

namespace Nowadays.Infrastructure.Services
{
    public interface IProjectService
    {
        Task<string> ProjectAdd(Project company);
        Task<string> ProjectDelete(Guid id);
        Task<string> ProjectUpdate(Project company);
    }
}