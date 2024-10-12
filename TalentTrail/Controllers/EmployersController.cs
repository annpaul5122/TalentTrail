using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Models;
using TalentTrail.Services;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployersController : ControllerBase
    {
        private static readonly ILog log=LogManager.GetLogger(typeof(EmployersController));
        private readonly IEmployerProfileService _profileService;
        public EmployersController(IEmployerProfileService profileService)
        {
            _profileService = profileService;
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] EmployerCreateDto employerCreateDto)
        {
            log.Info("CreateProfile method started.");
            if (!ModelState.IsValid)
            {
                log.Warn("Invalid model state.");
                return BadRequest(ModelState);
            }

            try
            {
                log.Info("Creating Employer and CompanyDetails objects.");
                var employer = new Employer
                {
                    UserId = employerCreateDto.UserId,
                    JobPosition = employerCreateDto.JobPosition,
                    IsThirdParty = employerCreateDto.IsThirdParty
                };
                var companyDetails = new CompanyDetails
                {
                    CompanyName = employerCreateDto.CompanyName,
                    CompanyWebUrl = employerCreateDto.CompanyWebUrl,
                    CompanyDescription = employerCreateDto.CompanyDescription,
                    CompanyLogo = employerCreateDto.CompanyLogo,
                    CompanyAddress = employerCreateDto.CompanyAddress,
                    Industry = employerCreateDto.Industry
                };

               
                var createdEmployer = await _profileService.CreateProfile(employer, companyDetails);
                log.Info($"Employer profile created successfully with EmployerId: {createdEmployer.EmployerId}");
                return Ok(new{message = "Employer registered successfully.",employerId = createdEmployer.EmployerId,userId = createdEmployer.UserId});
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating employer profile.", ex);
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Employer")]
        [HttpPut("{employerId}")]
        public async Task<IActionResult> UpdateProfile(int employerId, [FromBody] EmployerUpdateDto employerDto)
        {
            log.Info("UpdateProfile method started.");
            if (!ModelState.IsValid)
            {
                log.Warn("Invalid model state.");
                return BadRequest(ModelState);
            }

            try
            {
                var updatedEmployer = new Employer
                {
                    JobPosition = employerDto.JobPosition,
                    IsThirdParty = employerDto.IsThirdParty,
                    CompanyId = employerDto.CompanyId,
                    Users = new Users
                    {
                        FirstName = employerDto.FirstName,
                        LastName = employerDto.LastName,
                        Email = employerDto.Email
                    }
                };

                var result = await _profileService.UpdateProfile(employerId, updatedEmployer);
                log.Info($"{updatedEmployer.Users.FirstName} profile has been updated.");

                if (result == null)
                {
                    return NotFound("Employer not found.");
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating employer profile.", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize(Roles = "Employer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ViewProfile(int id)
        {
            try
            {
                var employerProfile = await _profileService.ViewProfile(id);
                if (employerProfile == null)
                {
                    return NotFound(new { message = "Employer profile not found" });
                }
                log.Info("Employer's profile has been viewed.");
                return Ok(employerProfile);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while displaying employer profile.", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Employer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            try
            {
                await _profileService.DeleteProfile(id);
                log.Info($"Employer Profile deleted: {id}");
                return Ok(new { message = "Account deleted successfully" });
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while deleting employer profile.", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("CheckProfile/{userId}")]
        public async Task<IActionResult> CheckProfile(int userId)
        {
            try
            {
                var employer = await _profileService.GetEmployerProfileByUserId(userId);

                if (employer != null)
                {
                    return Ok(new { exists = true, employerId = employer.EmployerId });
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
    }
}
