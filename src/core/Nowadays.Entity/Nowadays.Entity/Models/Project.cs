using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Entity.Models
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Issue> Issues { get; set; }
    }
}
