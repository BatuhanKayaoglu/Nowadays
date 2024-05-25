using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nowadays.Entity.Models;

namespace Nowadays.Infrastructure.Services
{
    public interface IIssueService
    {
        Task<string> IssueAdd(Issue issue);
        Task<string> IssueDelete(Guid id);
        Task<string> IssueUpdate(Issue issue);
    }
}