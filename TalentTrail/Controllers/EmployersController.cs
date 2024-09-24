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

        private readonly IEmployerProfileService _profileService;
        public EmployersController(IEmployerProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] EmployerCreateDto employerCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
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
                return Ok(new{message = "Employer registered successfully.",employerId = createdEmployer.EmployerId,userId = createdEmployer.UserId});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Employer,Job Seeker,Admin")]
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
                return Ok(employerProfile);
            }
            catch (Exception ex)
            {
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
                return Ok(new { message = "Profile deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
