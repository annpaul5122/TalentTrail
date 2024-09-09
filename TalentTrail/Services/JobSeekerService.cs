using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly TalentTrailDbContext _dbContext;

        public JobSeekerService(TalentTrailDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JobSeeker> CreateProfile(JobSeeker jobSeeker)
        {
            var existingUser = await _dbContext.Users.FindAsync(jobSeeker.UserId);
            if (existingUser == null)
            {
                throw new Exception("Invalid User ID.");
            }

            _dbContext.JobSeekers.Add(jobSeeker);
            await _dbContext.SaveChangesAsync();

            return jobSeeker;
        }

        public async Task<JobSeekerProfileDto> ViewProfile(int seekerId)
        {
            var jobSeeker = await _dbContext.JobSeekers
                .Include(js => js.User)
                .Include(js => js.Resumes)
                .Include(js => js.Application)
                .Include(js => js.Recommendations)
                .FirstOrDefaultAsync(js => js.SeekerId == seekerId);

            if (jobSeeker == null)
            {
                throw new Exception("Job Seeker not found.");
            }

            var profileDto = new JobSeekerProfileDto
            {
                FirstName = jobSeeker.User.FirstName,
                LastName = jobSeeker.User.LastName,
                Email = jobSeeker.User.Email,
                PhoneNumber = jobSeeker.PhoneNumber,
                ProfileSummary = jobSeeker.ProfileSummary,
                ResumePath = jobSeeker.ResumePath,
                Education = jobSeeker.Education,
                Experience = jobSeeker.Experience,
                Skills = jobSeeker.Skills,
                Certifications = jobSeeker.Certifications,
                LanguagesKnown = jobSeeker.LanguagesKnown,
                CreatedAt = jobSeeker.CreatedAt,
                LastUpdatedAt = jobSeeker.LastUpdatedAt
            };

            return profileDto;
        }

        public async Task DeleteProfile(int seekerId)
        {
            var jobSeeker = await _dbContext.JobSeekers
                .Include(js => js.User)
                .Include(js => js.Resumes)
                .Include(js => js.Application)
                .Include(js => js.Recommendations)
                .FirstOrDefaultAsync(js => js.SeekerId == seekerId);

            if (jobSeeker == null)
            {
                throw new Exception("Job Seeker not found.");
            }

            _dbContext.JobSeekers.Remove(jobSeeker);

            if (jobSeeker.User != null)
            {
                _dbContext.Users.Remove(jobSeeker.User);
            }

            // Optionally remove related entities like Resumes, Applications, Recommendations if necessary
            _dbContext.Resumes.RemoveRange(jobSeeker.Resumes);
            _dbContext.JobApplications.RemoveRange(jobSeeker.Application);
            _dbContext.Recommendations.RemoveRange(jobSeeker.Recommendations);

            await _dbContext.SaveChangesAsync();
        }
    }
}
