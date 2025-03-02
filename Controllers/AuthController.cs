using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(UserService userService, IConfiguration configuration) {
            _userService = userService;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel) {
            var user = await _userService.GetByEmailAsync(loginModel.Email);
            if (user == null || user.Password != loginModel.Password) {
                return Unauthorized(new { message = "Correo o contraseña incorrectos." });
            }

            if (string.IsNullOrEmpty(user.Id)) {
                return StatusCode(500, new { message = "El usuario no tiene un Id válido." });
            }

            await _userService.UpdateLastAccessAsync(user.Id);

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user) {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id ?? throw new ArgumentNullException(nameof(user.Id))),
                new Claim(JwtRegisteredClaimNames.Email, user.CorreoElectronico)
            };

            var key = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            var issuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
            var audience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
