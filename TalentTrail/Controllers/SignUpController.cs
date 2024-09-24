using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentTrail.Dto;
using TalentTrail.Enum;
using TalentTrail.Models;
using TalentTrail.Services;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private ISignUpService _signUpService;
        public SignUpController(ISignUpService signUpService)
        {
            _signUpService = signUpService;

        }

        [HttpPost("signup/employer")]
        public async Task<IActionResult> SignUpEmployer([FromBody] UsersDto userDto)
        {
            try
            {
                var user = new Users
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    CreatedAt = DateTime.UtcNow,
                    Role = Roles.Employer
                };

                var registeredUser = await _signUpService.SignUpUserAsync(user);
                return Ok(new { message = "Employer registered successfully.", userId = registeredUser.UserId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("signup/jobseeker")]
        public async Task<IActionResult> SignUpJobSeeker([FromBody] UsersDto userDto)
        {
            try
            {
                var user = new Users
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    CreatedAt = DateTime.UtcNow,
                    Role = Roles.JobSeeker
                };

                var registeredUser = await _signUpService.SignUpUserAsync(user);
                return Ok(new { message = "Job Seeker registered successfully.", userId = registeredUser.UserId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("signup/admin")]
        public async Task<IActionResult> SignUpAdmin([FromBody] UsersDto userDto)
        {
            try
            {
                var user = new Users
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    CreatedAt = DateTime.UtcNow,
                    Role = Roles.Admin
                };

                var registeredUser = await _signUpService.SignUpUserAsync(user);
                return Ok(new { message = "Admin registered successfully.", userId = registeredUser.UserId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
