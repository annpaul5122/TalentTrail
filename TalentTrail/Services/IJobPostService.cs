using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IJobPostService
    {
        Task<JobPost> CreateJobPost(JobPost jobPost);
        Task<JobPost> UpdateJobPost(int jobId, JobPost updatedJobPost);
        Task<JobPostDto> GetJobPostById(int jobId);
        Task<List<JobPostDto>> GetAllJobPosts(); 
        Task<List<JobPostDto>> GetJobPostsByEmployerId(int employerId);
        Task DeleteJobPost(int jobId);
    }
}
