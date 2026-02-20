using Microsoft.AspNetCore.Http;
using SafeRoute.Domain.Currents.Interfaces;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Currents.Implementations
{
    public class CurrentCompany : ICurrentCompany
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentCompany(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int CompanyId
        {
            get
            {
                var raw = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirstValue("company_id");

                return int.TryParse(raw, out var id) ? id : 0;
            }
        }
    }
}
