using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Common.Authentication;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Infrastructure.Authentication
{
    internal class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(
            string userId,
            string email,
            IList<string> roles)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email)
        };

            claims.AddRange(
                roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}



