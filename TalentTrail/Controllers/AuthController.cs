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
        public AuthController(IConfiguration configuration, TalentTrailDbContext conn)
        {
            this._config = configuration;
            _con = conn;
        }

        [NonAction]
        public Users Validate(string email, string password)
        {
            Users user = _con.Users.FirstOrDefault(i => i.Email == email);

            if (user == null)
            {
                throw new ArgumentException("User not found with the provided email.");
            }


            if (user.Password == password)
            {
                return user; 
            }
            else
            {
                throw new ArgumentException("Invalid password.");
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult UserAuth([FromBody] LoginRequestDto loginRequest)
        {
            IActionResult response = Unauthorized();

            var s = Validate(loginRequest.Email,loginRequest.Password);
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
                    new Claim(ClaimTypes.NameIdentifier, s.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub,s.FirstName),
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

        public class LoginRequestDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

    }

}
