﻿using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Models;
using TalentTrail.Services;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ResumesController));
        private readonly IResumeService _resumeService;

        public ResumesController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpPost]
        public async Task<IActionResult> CreateResume([FromBody] Resume resume)
        {
            try
            {
                var createdResume = await _resumeService.CreateResume(resume);
                log.Info("Resume added successfully.");
                return Ok(new { message = "Resume uploaded successfully.", resumeId = createdResume.ResumeId });
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while adding resume : ", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpGet("{seekerId}")]
        public async Task<IActionResult> GetAllResumes(int seekerId)
        {
            try
            {
                var resumes = await _resumeService.GetAllResumesOfJobSeeker(seekerId);
                return Ok(resumes);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpPut]
        public async Task<IActionResult> UpdateResumePath([FromBody] Resume resume)
        {
            try
            {
                var updatedResume = await _resumeService.UpdateResumePath(resume);
                log.Info("Resume updated successfully.");
                return Ok(updatedResume);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating resume : ", ex);
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpDelete("{resumeId}")]
        public async Task<IActionResult> DeleteResume(int resumeId)
        {
            try
            {
                await _resumeService.DeleteResume(resumeId);
                log.Info("Resume deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while deleting resume : ", ex);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
