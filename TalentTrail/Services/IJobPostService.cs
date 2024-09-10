using TalentTrail.Dto;
using TalentTrail.Enum;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IJobPostService
    {
        public Task<JobPost> CreateJobPost(JobPost jobPost);
        public Task<JobPost> UpdateJobPost(int jobId, JobPost updatedJobPost);
        public Task<JobPostDto> GetJobPostById(int jobId);
        public Task<List<JobPostDto>> GetAllJobPosts(); 
        public Task<List<JobPostDto>> GetJobPostsByEmployerId(int employerId);
        public Task DeleteJobPost(int jobId);
        public Task UpdateApplicationStatus(int applicationId, ApplicationStatus newStatus);



    }
}
