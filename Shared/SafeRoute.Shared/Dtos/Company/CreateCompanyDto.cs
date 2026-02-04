using SafeRoute.Shared.Enums.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Dtos.Company
{
    public class CreateCompanyDto
    {
        [Required(ErrorMessage = "Registro é requerido!")]
        public string Registry { get; set; }
        [Required(ErrorMessage = "Razão social é requerido!")]
        public string LegalName { get; set; }
        [Required(ErrorMessage = "Nome fantasia é requerido!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Status é requerido!")]
        public StatusBasicEnum StatusCompany { get; set; }
    }
}
