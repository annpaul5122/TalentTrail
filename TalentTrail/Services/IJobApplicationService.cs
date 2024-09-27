using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IJobApplicationService
    {
        public Task<JobApplication> CreateJobApplication(ApplyJobDto applyJobDto);
        public Task<JobApplicationDto> GetJobApplicationById(int applicationId);
        public Task<List<JobApplicationDto>> GetAllJobApplicationsByJobSeeker(int seekerId);
        public Task<List<JobApplicationDto>> GetAllJobApplicationsByJobPosts(int jobId);
        public Task UpdateJobApplication(ApplyJobDto applyJobDto);
        public Task DeleteJobApplication(int id);
    }
}
