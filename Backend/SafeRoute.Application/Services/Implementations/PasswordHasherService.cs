using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Domain.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly IPasswordHasher _passwordHasher;

        public PasswordHasherService(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public (string Hash, string Salt) HashPassword(string senha)
        {
            return _passwordHasher.HashPassword(senha);
        }

        public bool Verify(string senha, string hash, string salt)
        {
            return _passwordHasher.Verify(senha, hash, salt);
        }
    }
}
