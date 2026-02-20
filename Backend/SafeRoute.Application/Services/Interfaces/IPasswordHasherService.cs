using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IPasswordHasherService
    {
        (string Hash, string Salt) HashPassword(string senha);
        bool Verify(string senha, string hash, string salt);
    }
}
