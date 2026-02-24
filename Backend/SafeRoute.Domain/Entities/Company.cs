using SafeRoute.Shared.Enums.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string Registry { get; set; }
        public string LegalName { get; set; }
        public string Name { get; set; }
        public StatusBasicEnum StatusCompany { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
