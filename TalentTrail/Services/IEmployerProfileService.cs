using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IEmployerProfileService
    {
        public Task<Employer> CreateProfile (Employer employer,CompanyDetails companyDetails);
        public Task DeleteProfile(int employerId);
        public Task<EmployerProfileDto> ViewProfile(int employerId);


    }
}
