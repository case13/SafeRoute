using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Currents.Interfaces
{
    public interface ICurrentUser
    {
        int UserId { get; }
        string Email { get; }
        string Name { get; }
        string UserType { get; }
    }
}
