using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IEmployerProfileService
    {
        Task<Employer> CreateProfile (Employer employer,CompanyDetails companyDetails);
        //Task<Employer> UpdateProfile (Employer employer);
        Task DeleteProfile(int employerId);
        Task<EmployerProfileDto> ViewProfile(int employerId);


    }
}
