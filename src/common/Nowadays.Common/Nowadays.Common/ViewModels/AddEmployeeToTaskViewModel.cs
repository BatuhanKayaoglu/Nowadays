using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Common.ViewModels
{
    public class AddEmployeeToTaskViewModel
    {
        public Guid IssueId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
