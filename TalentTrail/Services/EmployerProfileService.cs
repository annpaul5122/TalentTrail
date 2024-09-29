using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class EmployerProfileService : IEmployerProfileService
    {
        private readonly TalentTrailDbContext _dbContext;
        private readonly IEmailService _emailService;
        public EmployerProfileService(TalentTrailDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;

        }

        public async Task<Employer> CreateProfile(Employer employer,CompanyDetails companyDetails)
        {
            var existingUser = await _dbContext.Users.FindAsync(employer.UserId);
            if (existingUser == null)
            {
                throw new ArgumentException("Invalid User ID.");
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

            var subject = "Profile Creation - Talent Trail";
            var body = $"Hello {existingUser.FirstName},\n\nYour profile as an employer has been created successfully.";

            try
            {
                await _emailService.SendEmailAsync(existingUser.Email, subject, body);
            }
            catch (Exception)
            {
              
            }

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
                throw new ArgumentException("Employer not found.");
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
                throw new ArgumentException("Employer not found.");
            }
            var user = employer.Users;

            _dbContext.Employers.Remove(employer);

            if (user != null)
            {
                _dbContext.Users.Remove(user); 
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Employer> GetEmployerProfileByUserId(int userId)
        {
            return await _dbContext.Employers.FirstOrDefaultAsync(e => e.UserId == userId);
        }


    }
}
