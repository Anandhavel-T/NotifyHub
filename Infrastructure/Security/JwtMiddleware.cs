using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using static System.Net.Mime.MediaTypeNames;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System;
using System.Linq;
using System.Security.Claims;

namespace NotifyHub.Infrastructure.Security
{
    public class JwtMiddleware : OwinMiddleware
    {
        private readonly string _secret;

        public JwtMiddleware(OwinMiddleware next) : base(next)
        {
            _secret = WebConfigurationManager.AppSettings["Jwt:Secret"];
        }

        public override async Task Invoke(IOwinContext context)
        {
            var token = context.Request.Headers.Get("Authorization")?.Split(' ').Last();

            if (!string.IsNullOrEmpty(token))
            {
                AttachUserToContext(context, token);
            }

            await Next.Invoke(context);
        }

        private void AttachUserToContext(IOwinContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                // Store the user ID in the Owin Context
                context.Set("UserId", userId);
            }
            catch
            {
                // Token validation failed, do nothing
            }
        }
    }
}
