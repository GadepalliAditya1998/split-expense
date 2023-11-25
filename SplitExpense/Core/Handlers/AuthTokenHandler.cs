using Microsoft.IdentityModel.Tokens;
using SplitExpense.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SplitExpense.Core.Handlers
{
    public class AuthTokenHandler : IAuthTokenHandler
    {
        private readonly IConfiguration configuration;
        public AuthTokenHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public TokenUserInfo? GetTokenUserInfo(string token)
        {
            TokenUserInfo? tokenUser = null;

            try
            {
                var jwtToken = this.ExtractToken(token);
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                
                if(userId > 0)
                {
                    tokenUser = new TokenUserInfo()
                    {
                        UserId = userId
                    };
                }
            }
            catch (Exception)
            {
                return null;
            }

            return tokenUser;
        }

        private JwtSecurityToken ExtractToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.configuration["JWT:Key"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
    }
}
