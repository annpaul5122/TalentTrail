using Microsoft.EntityFrameworkCore;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class ResumeService : IResumeService
    {
        private readonly TalentTrailDbContext _dbContext;

        public ResumeService(TalentTrailDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Resume> CreateResume(Resume resume)
        {
            var jobSeeker = await _dbContext.JobSeekers.FindAsync(resume.SeekerId);
            if (jobSeeker == null)
            {
                throw new ArgumentException("Job Seeker not found.");
            }

            resume.CreatedAt = DateTime.UtcNow;
            _dbContext.Resumes.Add(resume);
            await _dbContext.SaveChangesAsync();

            return resume;
        }

        public async Task<List<Resume>> GetAllResumesOfJobSeeker(int seekerId)
        {
            var resumes = await _dbContext.Resumes
                .Where(r => r.SeekerId == seekerId)
                .ToListAsync();

            if (resumes == null || !resumes.Any())
            {
                throw new ArgumentException("No resumes found for the given Job Seeker.");
            }

            return resumes;
        }

        public async Task<Resume> UpdateResumePath(Resume resume)
        {
            var existingResume = await _dbContext.Resumes
                .FirstOrDefaultAsync(r => r.ResumeId == resume.ResumeId);

            if (existingResume == null)
            {
                throw new ArgumentException("Resume not found.");
            }

            existingResume.ResumePath = resume.ResumePath;
            existingResume.UpdatedAt = DateTime.UtcNow;

            _dbContext.Resumes.Update(existingResume);
            await _dbContext.SaveChangesAsync();

            return existingResume;
        }

        public async Task DeleteResume(int resumeId)
        {
            var resume = await _dbContext.Resumes.FindAsync(resumeId);
            if (resume == null)
            {
                throw new ArgumentException("Resume not found.");
            }

            _dbContext.Resumes.Remove(resume);
            await _dbContext.SaveChangesAsync();
        }
    }
}
