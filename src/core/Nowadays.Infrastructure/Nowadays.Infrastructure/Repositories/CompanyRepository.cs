using Nowadays.Entity.Models;
using Nowadays.Infrastructure.Context;
using Nowadays.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(NowadaysContext dbContext) : base(dbContext)
        {

        }
    }
}
