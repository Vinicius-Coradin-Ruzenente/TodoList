using TodoList.Model;
using TodoList.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TodoList.Helpers
{
    public class JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        private readonly RequestDelegate _next = next;
        private readonly AppSettings _appSettings = appSettings.Value;

        public async Task Invoke(HttpContext context, IUserServices userService)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            if(token != null)
            {
                await AttachUserToContext(context, userService, token);
            }
            await _next(context);
        }                

        public async Task AttachUserToContext(HttpContext context, IUserServices userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                context.Items["User"] = await userService.GetUserById(userId);
            }
            catch
            {

            }
        }
    }
}
