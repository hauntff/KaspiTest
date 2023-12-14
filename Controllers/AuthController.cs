using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using KaspiTest.Models;
using System.Text;
using KaspiTest.ApplicationContext;

namespace KaspiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly string _audience; // потребитель токена
        private readonly string _issuer; // издатель токена
        private readonly string _key; // ключ для шифрации
        private readonly int _lifetime;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            _audience = configuration["Auth:Audience"] ?? "";
            _issuer = configuration["Auth:Issuer"] ?? "";
            _key = configuration["Auth:Key"] ?? "";
            _lifetime = int.Parse(configuration["Auth:Lifetime"] ?? "1");
            _context = context;
        }

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            notBefore: now,
            claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_lifetime)),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)),
                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            Person? person = _context.Persons.FirstOrDefault(x => x.Login == username && x.Password == password);

            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
