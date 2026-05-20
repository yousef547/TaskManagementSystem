using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Common.Dtos.Response
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;

        public string Email { get; set; } = null!;

        public IList<string> Roles { get; set; } = [];
    }
}
