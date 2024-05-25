using Nowadays.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Entity.Models
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Issue> Issues { get; set; }

    }
}
