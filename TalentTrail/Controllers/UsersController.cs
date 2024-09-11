using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentTrail.Models;
using TalentTrail.Services;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Employer,Job Seeker,Admin")]
        [HttpPost("{userId}/reset-password")]
        public async Task<IActionResult> ResetPassword(int userId, [FromBody] string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return BadRequest("Password cannot be empty.");
            }

            try
            {
                await _userService.ResetPassword(userId, newPassword);
                return Ok(new { message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
    }
}
