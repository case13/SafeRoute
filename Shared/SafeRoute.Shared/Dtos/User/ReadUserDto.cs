using SafeRoute.Shared.Enums.Status;
using SafeRoute.Shared.Enums.Tipos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Dtos.User
{
    public class ReadUserDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserTypeEnum UserType { get; set; }
        public StatusBasicEnum UserStatus { get; set; }
    }
}
