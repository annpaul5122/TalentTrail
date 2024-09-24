using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Models;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDetailsController : ControllerBase
    {
        private readonly TalentTrailDbContext _dbContext;
        public CompanyDetailsController(TalentTrailDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDetails>>> GetCompanyDetails()
        {
            try
            {
                var companies = await _dbContext.CompanyDetails.ToListAsync();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
