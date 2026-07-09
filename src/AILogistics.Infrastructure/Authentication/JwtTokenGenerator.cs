using AILogistics.Application.DTOs;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public JwtTokenResultDto GenerateToken(User user)
        {
            JwtSettings jwtSettings = new JwtSettings
            {
                SecretKey = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt Key is missing. "),

                Issuer = _config["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Jwt issuer is missing. "),

                Audience = _config["Jwt:Audience"]
                ?? throw new InvalidOperationException("Jwt Audience is missing. "),

                AccessTokenExpiryMinutes = int.Parse(_config["Jwt:ExpiresInMinutes"] ?? "60")
            };
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

            var credentials = new SigningCredentials
            (
                key,
                SecurityAlgorithms.HmacSha256
                );

            var expiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpiryMinutes);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpiryMinutes),
                signingCredentials: credentials

                );
            string jwtString = new JwtSecurityTokenHandler().WriteToken(token);

            JwtTokenResultDto jwtToken = new JwtTokenResultDto
            {
                Token = jwtString,
                ExpiresAt = expiresAt
            };
            return jwtToken;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
