using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Entity.Models
{
    public class Report: BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Attachments { get; set; }
        public string BugUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
