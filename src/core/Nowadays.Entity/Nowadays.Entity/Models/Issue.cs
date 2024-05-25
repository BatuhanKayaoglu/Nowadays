using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Entity.Models
{
    public class Issue : BaseEntity
    {
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public ICollection<Employee> Assignees { get; set; }
    }
}
