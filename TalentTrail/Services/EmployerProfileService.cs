using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class EmployerProfileService : IEmployerProfileService
    {
        private readonly TalentTrailDbContext _dbContext;
        public EmployerProfileService(TalentTrailDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employer> CreateProfile(Employer employer,CompanyDetails companyDetails)
        {
            var existingUser = await _dbContext.Users.FindAsync(employer.UserId);
            if (existingUser == null)
            {
                throw new Exception("Invalid User ID.");
            }

            var existingCompany = await _dbContext.CompanyDetails
                .FirstOrDefaultAsync(c => c.CompanyName == companyDetails.CompanyName);

            if (existingCompany != null)
            {
                employer.CompanyId = existingCompany.CompanyId;
            }
            else
            {
                _dbContext.CompanyDetails.Add(companyDetails);
                await _dbContext.SaveChangesAsync();
                employer.CompanyId = companyDetails.CompanyId;
            }
            _dbContext.Employers.Add(employer);
            await _dbContext.SaveChangesAsync();

            return employer;
        }

        public async Task<EmployerProfileDto> ViewProfile(int employerId)
        {
            var employer = await _dbContext.Employers
                .Include(e => e.CompanyDetails)
                .Include(e => e.Users)
                .FirstOrDefaultAsync(e => e.EmployerId == employerId);

            if (employer == null)
            {
                throw new Exception("Employer not found.");
            }

            var profileDto = new EmployerProfileDto
            {
                FirstName = employer.Users.FirstName,
                LastName = employer.Users.LastName,
                Email = employer.Users.Email,
                JobPosition = employer.JobPosition.ToString(),
                IsThirdParty = employer.IsThirdParty,
                CompanyDetails = employer.IsThirdParty ? null : new CompanyDetailsDto
                {
                    CompanyName = employer.CompanyDetails.CompanyName,
                    CompanyWebUrl = employer.CompanyDetails.CompanyWebUrl,
                    CompanyDescription = employer.CompanyDetails.CompanyDescription,
                    CompanyLogo = employer.CompanyDetails.CompanyLogo,
                    CompanyAddress = employer.CompanyDetails.CompanyAddress,
                    Industry = employer.CompanyDetails.Industry
                }
            };
            return profileDto;
        }

        public async Task DeleteProfile(int employerId)
        {
            var employer = await _dbContext.Employers
                               .Include(e => e.Users)
                               .FirstOrDefaultAsync(e => e.EmployerId == employerId);

            if (employer == null)
            {
                throw new Exception("Employer not found.");
            }
            var user = employer.Users;

            _dbContext.Employers.Remove(employer);

            if (user != null)
            {
                _dbContext.Users.Remove(user); 
            }

            await _dbContext.SaveChangesAsync();
        }


    }
}
