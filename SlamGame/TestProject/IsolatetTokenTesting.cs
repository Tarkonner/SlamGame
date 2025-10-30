using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TokenUtils
{
    public class IsolatetTokenTesting
    {

        [Fact]
        public void SecretKeyIsSet()
        {
            var secret = Environment.GetEnvironmentVariable("Slam");
            Assert.False(string.IsNullOrEmpty(secret));
        }

        [Fact]
        public void GenerateToken_Success()
        {
            var token = TestingTokenGenerator.GenerateJwtToken();
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void ValidateToken_Success()
        {
            var token = TestingTokenGenerator.GenerateJwtToken();
            var principal = TestingTokenGenerator.ValidateJwtToken(token);

            Assert.NotNull(principal);
            Assert.True(principal.Identity?.IsAuthenticated);
        }

        [Fact]
        public void ExtractClaimFromToken_Success()
        {
            var token = TestingTokenGenerator.GenerateJwtToken();
            var nameClaim = TestingTokenGenerator.GetClaimFromToken(token, ClaimTypes.Name);

            Assert.Equal("TestUser", nameClaim);
        }
    }
}
