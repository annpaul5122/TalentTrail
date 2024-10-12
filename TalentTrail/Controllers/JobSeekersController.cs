using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Enum;
using TalentTrail.Models;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekersController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(JobSeekersController));
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
                var createdJobSeeker = await _jobSeekerService.CreateProfile(profileDto.JobSeeker,profileDto.ResumePaths,profileDto.Educations,profileDto.Certifications);
                log.Info($"Jobseeker profile {profileDto.JobSeeker.SeekerId} created successfully.");
                return Ok(createdJobSeeker);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating job seeker profile : ", ex);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpPut("{seekerId}")]
        public async Task<IActionResult> UpdateProfile(int seekerId, [FromBody] JobSeekerUpdateDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request payload.");
            }

            try
            {
                var updatedJobSeeker = new JobSeeker
                {
                    User = new Users
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email
                    },
                    PhoneNumber = request.PhoneNumber,
                    ProfileSummary = request.ProfileSummary,
                    Experience = request.Experience,
                    Skills = request.Skills,
                    LanguagesKnown = request.LanguagesKnown
                };

                var updatedJobSeekerProfile = await _jobSeekerService.UpdateProfile(
                    seekerId,
                    updatedJobSeeker,
                    request.Educations,
                    request.Certifications
                );
                log.Info($"Jobseeker profile {seekerId} updated successfully.");
                return Ok(updatedJobSeekerProfile);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating the jobseeker profile : ", ex);
                return StatusCode(500, "An error occurred while updating the profile. Please try again later.");
            }
        }


        [Authorize(Roles = "Job Seeker")]
        [HttpDelete("{seekerId}")]

        public async Task<IActionResult> DeleteProfile(int seekerId)
        {
            try
            {
                await _jobSeekerService.DeleteProfile(seekerId);
                log.Info($"Jobseeker profile {seekerId} deleted successfully.");
                return Ok(new { message = "Job Seeker profile deleted successfully." });
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while deleting the jobseeker profile : ", ex);
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpGet("{seekerId}")]
        public async Task<IActionResult> ViewProfile(int seekerId)
        {
            try
            {
                var profileDto = await _jobSeekerService.ViewProfile(seekerId);
                log.Info($"Jobseeker profile {seekerId} retrieved successfully.");
                return Ok(profileDto);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while displaying the jobseeker profile : ", ex);
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchJobPosts([FromQuery] string? jobTitle)
        {
            try
            {
                var jobPosts = await _jobSeekerService.SearchJobPosts(jobTitle);
                if (jobPosts == null || !jobPosts.Any())
                {
                    return NotFound("No job posts found with the given criteria.");
                }
                log.Info($"Job Posts retrieved successfully based on the job title : {jobTitle}");
                return Ok(jobPosts);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while fetching job posts based on the search request", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpGet("filter")]
        public async Task<IActionResult> JobPostFilter([FromQuery] string? jobTitle,[FromQuery] string? industry, [FromQuery] string? requirements, [FromQuery] string? location, [FromQuery] EmploymentType? employmentType)
        {
            try
            {
                var jobPosts = await _jobSeekerService.JobPostFilter(jobTitle,industry,requirements,location,employmentType);
                if (jobPosts == null || !jobPosts.Any())
                {
                    return NotFound("No job posts found with the given criteria.");
                }
                log.Info("Job Posts retrieved successfully based on the filters.");
                return Ok(jobPosts);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while fetching job posts based on the filters", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("CheckProfile/{userId}")]
        public async Task<IActionResult> CheckProfile(int userId)
        {
            try
            {
                var seeker = await _jobSeekerService.GetSeekerProfileByUserId(userId);

                if (seeker != null)
                {
                    return Ok(new { exists = true, seekerId = seeker.SeekerId });
                }
                else
                {
                    return Ok(new { exists = false });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpGet("appliedJobs")]
        public async Task<IActionResult> GetAppliedJobs([FromQuery] int seekerId)
        {
            if (seekerId <= 0)
            {
                return BadRequest("Valid Job Seeker ID is required.");
            }

            try
            {
                var appliedJobIds = await _jobSeekerService.GetAppliedJobsAsync(seekerId);
                return Ok(appliedJobIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}
