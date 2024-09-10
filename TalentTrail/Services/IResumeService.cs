using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IResumeService
    {
        public Task<Resume> CreateResume(Resume resume);
        public Task<List<Resume>> GetAllResumesOfJobSeeker(int seekerId);
        public Task<Resume> UpdateResumePath(Resume resume);
        public Task DeleteResume(int resumeId);
    }
}
