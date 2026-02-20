using Microsoft.AspNetCore.Http;
using SafeRoute.Domain.Currents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Currents.Implementations
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var raw = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                return int.TryParse(raw, out var id) ? id : 0;
            }
        }

        public string Email =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.Email);

        public string Name =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.Name);

        public string UserType =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirstValue("user_type");
    }
}
