using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenUtils
{
    public static class TestingTokenGenerator
    {
        // Secret key read from environment variable "Slam"
        private static readonly string SecretKey = Environment.GetEnvironmentVariable("Slam")
                                                   ?? throw new InvalidOperationException("JWT secret key not set in environment variable 'Slam'");

        private const string Issuer = "TestIssuer";
        private const string Audience = "TestAudience";

        public static string GenerateJwtToken(
            string username = "TestUser",
            IEnumerable<Claim>? extraClaims = null,
            int expiresMinutes = 30)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new(ClaimTypes.Role, "Tester")
            };

            if (extraClaims != null)
                claims.AddRange(extraClaims);

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsPrincipal ValidateJwtToken(string token)
        {
            var secretKey = Environment.GetEnvironmentVariable("Slam")
                            ?? throw new InvalidOperationException("JWT secret key not set in environment variable 'Slam'");

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };

            var handler = new JwtSecurityTokenHandler();
            return handler.ValidateToken(token, validationParameters, out var validatedToken);
        }

        public static string GetClaimFromToken(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            return jwt.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}
