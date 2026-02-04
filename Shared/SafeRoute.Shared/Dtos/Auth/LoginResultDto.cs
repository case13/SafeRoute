using SafeRoute.Shared.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeRoute.Shared.Dtos.Auth
{
    public class LoginResultDto
    {
        public int UserId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
