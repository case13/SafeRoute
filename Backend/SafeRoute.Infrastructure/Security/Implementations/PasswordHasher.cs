using SafeRoute.Domain.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SafeRoute.Infrastructure.Security.Implementations
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;          // 128 bits
        private const int KeySize = 32;           // 256 bits
        private const int Iterations = 100_000;   // custo seguro atual

        public (string Hash, string Salt) HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password inválida.");

            byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hashBytes = KeyDerivation.Pbkdf2(
                password,
                saltBytes,
                KeyDerivationPrf.HMACSHA256,
                Iterations,
                KeySize
            );

            return (
                Convert.ToBase64String(hashBytes),
                Convert.ToBase64String(saltBytes)
            );
        }

        public bool Verify(string password, string hash, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] expectedHashBytes = Convert.FromBase64String(hash);

            byte[] actualHashBytes = KeyDerivation.Pbkdf2(
                password,
                saltBytes,
                KeyDerivationPrf.HMACSHA256,
                Iterations,
                KeySize
            );

            return CryptographicOperations.FixedTimeEquals(
                expectedHashBytes,
                actualHashBytes
            );
        }
    }
}
