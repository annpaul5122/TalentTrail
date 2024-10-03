using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using TalentTrail.Dto;
using TalentTrail.Models;
using TalentTrail.Services;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TalentTrailDbContext _dbContext;
        public UsersController(IUserService userService, TalentTrailDbContext dbContext)
        {
            _userService = userService;
            _dbContext = dbContext;

        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] EmailRequest emailRequest)
        {
            await _userService.SendPasswordResetEmail(emailRequest.Email);
            return Ok("Password reset email sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword reset)
        {
            await _userService.ResetPassword(reset.token,reset.newPassword,reset.confirmPassword);
            return Ok("Password has been reset.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")]

        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await _userService.DeleteUser(userId);
                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Employer,Job Seeker,Admin")]
        [HttpPut("{userId}")]

        public async Task<IActionResult> UpdateUserDetails(int userId, [FromBody] Users updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId != updatedUser.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                var updatedUserInfo = await _userService.UpdateUserDetails(updatedUser);
                return Ok(updatedUserInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Employer,Job Seeker,Admin")]
        [HttpGet("details/{userId}")]
        public async Task<ActionResult<UserDto>> GetUserDetails(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var userDto = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return Ok(userDto);
        }
    }
}
