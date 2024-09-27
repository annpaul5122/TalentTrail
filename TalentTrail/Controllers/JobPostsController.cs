using Microsoft.AspNetCore.Authorization;
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
    public class JobPostsController : ControllerBase
    {
        private readonly IJobPostService _jobPostService;

        public JobPostsController(IJobPostService jobPostService)
        {
            _jobPostService = jobPostService;
        }

        //[Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> CreateJobPost([FromBody] JobPost jobPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdJobPost = await _jobPostService.CreateJobPost(jobPost);
            return Ok(new { message = "Job post created successfully.", jobId = createdJobPost.JobId });
        }

        //[Authorize(Roles = "Employer")]
        [HttpPut("{jobId}")]
        public async Task<IActionResult> UpdateJobPost(int jobId, [FromBody] JobPost jobPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedJobPost = await _jobPostService.UpdateJobPost(jobId, jobPost);
                return Ok(new { message = "Job post updated successfully.", jobId = updatedJobPost.JobId });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        //[Authorize(Roles = "Employer,Job Seeker")]
        [HttpGet("jobId/{jobId}")]
        public async Task<IActionResult> GetJobPostById(int jobId)
        {
            try
            {
                var jobPost = await _jobPostService.GetJobPostById(jobId);
                return Ok(jobPost);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

       // [Authorize(Roles ="Employer,Job Seeker,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllJobPosts()
        {
            var jobPosts = await _jobPostService.GetAllJobPosts();
            return Ok(jobPosts);
        }

        //[Authorize(Roles = "Employer,Job Seeker")]
        [HttpGet("getJobPostByEmpId/{employerId}")]

        public async Task<IActionResult> GetJobPostsByEmployerId(int employerId)
        {
            try
            {
                var jobPosts = await _jobPostService.GetJobPostsByEmployerId(employerId);

                if (jobPosts == null || jobPosts.Count == 0)
                {
                    return NotFound(new { message = "No job posts found for the given employer." });
                }

                return Ok(jobPosts);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

       // [Authorize(Roles = "Employer")]
        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteJobPost(int jobId)
        {
            try
            {
                await _jobPostService.DeleteJobPost(jobId);
                return Ok(new { message = "Job post deleted successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

      //  [Authorize(Roles = "Employer")]
        [HttpPut("UpdateApplicationStatus")]
        public async Task<IActionResult> UpdateApplicationStatus([FromBody] UpdateApplicationStatusDto updateDto)
        {
            try
            {
                await _jobPostService.UpdateApplicationStatus(updateDto.ApplicationId, updateDto.NewStatus);
                return Ok("Application status updated successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
