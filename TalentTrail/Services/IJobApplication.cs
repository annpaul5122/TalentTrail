using System.Collections.Generic;
using System.Threading.Tasks;
using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IJobApplicationService
    {
        Task<JobApplicationDto> CreateJobApplicationAsync(JobApplicationDto jobApplicationDto);
        Task<JobApplicationDto> GetJobApplicationByIdAsync(int id);
        Task<IEnumerable<JobApplicationDto>> GetAllJobApplicationsAsync();
        Task UpdateJobApplicationAsync(JobApplicationDto jobApplicationDto);
        Task DeleteJobApplicationAsync(int id);
    }
}
