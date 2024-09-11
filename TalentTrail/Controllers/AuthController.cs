using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TalentTrail.Models;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IConfiguration _config;
        private readonly TalentTrailDbContext _con;
        private readonly IPasswordHasher<Users> _passwordHasher;
        public AuthController(IConfiguration configuration, TalentTrailDbContext conn, IPasswordHasher<Users> passwordHasher)
        {
            this._config = configuration;
            _con = conn;
            _passwordHasher = passwordHasher;
        }

        [NonAction]
        public Users Validate(string email, string password)
        {
            Users user = _con.Users.FirstOrDefault(i => i.Email == email);

            if (user == null)
            {
                throw new Exception("User not found with the provided email.");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                return user; 
            }
            else
            {
                throw new Exception("Invalid password.");
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult UserAuth(string Email, string Password)
        {
            IActionResult response = Unauthorized();

            var s = Validate(Email,Password);
            if (s != null)
            {

                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                var signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha512Signature);

                var subject = new ClaimsIdentity(new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Email,s.Email),
                    new Claim(ClaimTypes.Role, s.Role) // Assign role to the token
                    });

                var expires = DateTime.UtcNow.AddMinutes(10);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = signingCredentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return Ok(jwtToken);

            }
            return response;
        }
    }

}
