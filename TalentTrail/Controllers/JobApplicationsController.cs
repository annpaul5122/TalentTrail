﻿using log4net;
using Microsoft.AspNetCore.Authorization;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(JobApplicationsController));
        private readonly IJobApplicationService _jobApplicationService;
        public JobApplicationsController(IJobApplicationService jobApplicationService)
        {
            _jobApplicationService = jobApplicationService;
        }

        [Authorize(Roles = "Job Seeker")]
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
                log.Info($"Job application {jobApplication.ApplicationId} created successfully.");
                return Ok(new { message = "Job Application created successfully.", applicationId = jobApplication.ApplicationId });
            }
            catch (Exception ex)
            {
                log.Warn("Error occured while creating job application : ",ex);
                return BadRequest(new { message = ex.Message });
            }
        }

       // [Authorize(Roles = "Employer,Job Seeker")]
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

        [Authorize(Roles = "Job Seeker")]
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

        [Authorize(Roles = "Employer")]
        [HttpGet("GetApplicationByJobPost/{jobId}")]
        public async Task<IActionResult> GetAllJobApplicationsByJobPosts(int jobId)
        {
            try
            {
                var jobApplicationDtos = await _jobApplicationService.GetAllJobApplicationsByJobPosts(jobId);
                return Ok(jobApplicationDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobApplication(int id)
        {
            try
            {
                await _jobApplicationService.DeleteJobApplication(id);
                log.Info($"Job Application {id} deleted successfully.");
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                log.Error("Error occured while deleting job application : ", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
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
                log.Info("Job Application updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating job application : ", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Employer")]
        [HttpGet("DownloadApplicantPDF/{applicationId}")]
        public async Task<IActionResult> DownloadApplicantPDF(int applicationId)
        {
            try
            {
                var pdfBytes = await _jobApplicationService.GenerateApplicantPdfAsync(applicationId);
                return File(pdfBytes, "application/pdf", $"Applicant_Profile_{applicationId}.pdf");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Applicant not found.");
            }
        }
    }
}
