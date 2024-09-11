using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekersController : ControllerBase
    {
        private readonly IJobSeekerService _jobSeekerService;
        public JobSeekersController(IJobSeekerService jobSeekerService)
        {
            _jobSeekerService = jobSeekerService;
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpPost("ProfileCreation")]
        public async Task<IActionResult> CreateProfile([FromBody] CreateJobSeekerProfileDto profileDto)
        {
            try
            {
                var createdJobSeeker = await _jobSeekerService.CreateProfile(profileDto.JobSeeker,profileDto.ResumePaths);
                return Ok(createdJobSeeker);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpDelete("{seekerId}")]

        public async Task<IActionResult> DeleteProfile(int seekerId)
        {
            try
            {
                await _jobSeekerService.DeleteProfile(seekerId);
                return Ok(new { message = "Job Seeker profile deleted successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Employer,Job Seeker,Admin")]
        [HttpGet("{seekerId}")]
        public async Task<IActionResult> ViewProfile(int seekerId)
        {
            try
            {
                var profileDto = await _jobSeekerService.ViewProfile(seekerId);
                return Ok(profileDto);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchJobPosts([FromQuery] string? industry, [FromQuery] string? jobTitle, [FromQuery] string? jobLocation)
        {
            try
            {
                var jobPosts = await _jobSeekerService.SearchJobPosts(industry,jobTitle,jobLocation);
                if (jobPosts == null || !jobPosts.Any())
                {
                    return NotFound("No job posts found with the given criteria.");
                }
                return Ok(jobPosts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
