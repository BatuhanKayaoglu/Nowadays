using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.IRepositories
{
    public interface IUnitOfWork
    {
        ICompanyRepository Companies { get; }
        IProjectRepository Projects { get; }
        IEmployeeRepository Employees { get; }
        IIssueRepository Issue { get; }
        IReportRepository Report { get; }
        int SaveChanges();
    }
}
