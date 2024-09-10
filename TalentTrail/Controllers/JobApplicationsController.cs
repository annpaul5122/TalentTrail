using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentTrail.Dto;
using TalentTrail.Services;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;
        public JobApplicationsController(IJobApplicationService jobApplicationService)
        {
            _jobApplicationService = jobApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobApplication([FromBody] ApplyJobDto applyJobDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var jobApplication = await _jobApplicationService.CreateJobApplication(applyJobDto);
                return Ok(new { message = "Job Application created successfully.", applicationId = jobApplication.ApplicationId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

   
        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetJobApplicationById(int applicationId)
        {
            try
            {
                var jobApplicationDto = await _jobApplicationService.GetJobApplicationById(applicationId);
                return Ok(jobApplicationDto);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("GetApplicationByJobSeeker/{seekerId}")]
        public async Task<IActionResult> GetAllJobApplicationsByJobSeeker(int seekerId)
        {
            try
            {
                var jobApplicationDtos = await _jobApplicationService.GetAllJobApplicationsByJobSeeker(seekerId);
                return Ok(jobApplicationDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobApplication(int id)
        {
            try
            {
                await _jobApplicationService.DeleteJobApplication(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateJobApplication([FromBody] ApplyJobDto applyJobDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _jobApplicationService.UpdateJobApplication(applyJobDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
