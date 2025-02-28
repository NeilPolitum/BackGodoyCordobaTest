using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Services;

public class JwtMiddleware {
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration) {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context, UserService userService) {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null) {
            AttachUserToContext(context, userService, token);
        }

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, UserService userService, string token) {
        try {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("Jwt:Key", "La clave JWT no puede ser nula o vacía.");
            }
            var keyBytes = Encoding.ASCII.GetBytes(key);
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == "sub").Value;

            context.Items["User"] = userService.GetAsync(userId).Result;
        } catch {
            // Do nothing if JWT validation fails
        }
    }
}
