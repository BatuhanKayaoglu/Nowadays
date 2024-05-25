using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Entity.Models
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
