using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(
       string userId,
       string email,
       IList<string> roles);
    }
}
