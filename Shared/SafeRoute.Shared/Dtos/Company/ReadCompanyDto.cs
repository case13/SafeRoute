using SafeRoute.Shared.Enums.Status;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeRoute.Shared.Dtos.Company
{
    public class ReadCompanyDto
    {
        public int Id { get; set; }
        public string Registry { get; set; }
        public string LegalName { get; set; }
        public string Name { get; set; }
        public StatusBasicEnum StatusCompany { get; set; }
    }
}
