using SafeRoute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Repositories.Interfaces
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<Company?> GetByRegistryAsync(string registry);
    }
}
