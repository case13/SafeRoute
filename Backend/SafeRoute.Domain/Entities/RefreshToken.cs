using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        // Properties of domain not mapped to database
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsValid => !IsExpired && !IsRevoked;

        public int UserId { get; set; }
        public User User { get; set; }

        public void Revoke()
        {
            if (IsRevoked)
                return;

            RevokedAt = DateTime.UtcNow;
        }

    }
}
