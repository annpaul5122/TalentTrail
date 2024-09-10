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

        public async Task<JobSeeker> CreateProfile(JobSeeker jobSeeker, List<string> resumePaths)
        {
            var existingUser = await _dbContext.Users.FindAsync(jobSeeker.UserId);
            if (existingUser == null)
            {
                throw new Exception("Invalid User ID.");
            }

            _dbContext.JobSeekers.Add(jobSeeker);
            await _dbContext.SaveChangesAsync();

            foreach (var resumePath in resumePaths)
            {
                var resume = new Resume
                {
                    SeekerId = jobSeeker.SeekerId, 
                    ResumePath = resumePath,
                    IsDefault = true,
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.Resumes.Add(resume);
            }
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
                ResumePath = jobSeeker.Resumes.Select(r => r.ResumePath).ToList(),
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

            _dbContext.Resumes.RemoveRange(jobSeeker.Resumes);
            _dbContext.JobApplications.RemoveRange(jobSeeker.Application);
            _dbContext.Recommendations.RemoveRange(jobSeeker.Recommendations);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<JobPostDto>> SearchJobPosts(string? industry, string? jobTitle, string? jobLocation)
        {
            var query = _dbContext.JobPosts.AsQueryable();

            if (!string.IsNullOrEmpty(industry))
            {
                query = query.Where(j => j.Industry == industry);
            }

            if (!string.IsNullOrEmpty(jobTitle))
            {
                query = query.Where(j => j.JobTitle.Contains(jobTitle));
            }

            if (!string.IsNullOrEmpty(jobLocation))
            {
                query = query.Where(j => j.JobLocation.Contains(jobLocation));
            }

            var jobPosts = await query.Include(j => j.Employer)
                              .Include(j => j.Employer.Users) 
                              .ToListAsync();

            return jobPosts.Select(j => new JobPostDto
            {
                EmployerName = j.Employer.Users.FirstName + " " + j.Employer.Users.LastName,
                JobTitle = j.JobTitle,
                JobDescription = j.JobDescription,
                JobRequirements = j.JobRequirements,
                JobLocation = j.JobLocation,
                SalaryRange = j.SalaryRange,
                EmploymentType = j.EmploymentType.ToString(),
                Industry = j.Industry,
                CreatedAt = j.CreatedAt,
                ApplicationDeadline = j.ApplicationDeadline,
                UpdatedAt = j.UpdatedAt
            }).ToList();
        }
    }
}
