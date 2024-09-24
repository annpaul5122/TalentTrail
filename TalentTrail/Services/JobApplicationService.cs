using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Enum;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly TalentTrailDbContext _dbContext;
     

        public JobApplicationService(TalentTrailDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JobApplication> CreateJobApplication(ApplyJobDto applyJobDto)
        {
            var jobSeeker = await _dbContext.JobSeekers
                .Include(js => js.Resumes)
                .FirstOrDefaultAsync(js => js.SeekerId == applyJobDto.seekerId);

            if (jobSeeker == null)
            {
                throw new ArgumentException("Job Seeker not found.");
            }

            var jobPost = await _dbContext.JobPosts.FindAsync(applyJobDto.jobId);
            if (jobPost == null)
            {
                throw new ArgumentException("Job Post not found.");
            }

            if(jobPost.ApplicationDeadline < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Cannot apply for the job post.");
            }

            var resume = jobSeeker.Resumes.FirstOrDefault(r => r.ResumeId == applyJobDto.resumeId);
            if (resume == null)
            {
                throw new ArgumentException("Resume not found or does not belong to the job seeker.");
            }

            var jobApplication = new JobApplication
            {
                SeekerId = applyJobDto.seekerId,
                JobId = applyJobDto.jobId,
                CoverLetter = applyJobDto.coverLetter,
                ResumePath = resume.ResumePath,
                ApplicationDate = DateTime.UtcNow.Date,
                ApplicationStatus = ApplicationStatus.Applied
            };

            _dbContext.JobApplications.Add(jobApplication);
            await _dbContext.SaveChangesAsync();

            return jobApplication;
        }

        public async Task<JobApplicationDto> GetJobApplicationById(int applicationId)
        {
            var application = await _dbContext.JobApplications
                .Include(ja => ja.jobSeeker)
                .Include(ja => ja.jobSeeker.User)
                .FirstOrDefaultAsync(ja => ja.ApplicationId==applicationId);

            if(application == null)
            {
                throw new ArgumentException("Job Application not found.");
            }

            var applicationDto = new JobApplicationDto()
            {
                ApplicationId = applicationId,
                SeekerName = application.jobSeeker.User.FirstName + " " + application.jobSeeker.User.LastName,
                JobId = application.JobId,
                CoverLetter = application.CoverLetter,
                ResumePath = application.ResumePath,
                ApplicationDate = application.ApplicationDate.Date,
                ApplicationStatus = application.ApplicationStatus.ToString()
            };

            return applicationDto;
        }

        public async Task<List<JobApplicationDto>> GetAllJobApplicationsByJobSeeker(int seekerId)
        {
            var jobApplications = await _dbContext.JobApplications
               .Include(ja => ja.jobSeeker)
               .Include(ja => ja.jobSeeker.User)
               .Where(ja => ja.SeekerId == seekerId)
               .ToListAsync();

            var jobApplicationDtos = jobApplications.Select(ja => new JobApplicationDto
            {
                ApplicationId = ja.ApplicationId,
                SeekerName = ja.jobSeeker.User.FirstName + " " + ja.jobSeeker.User.LastName,
                JobId = ja.JobId,
                CoverLetter = ja.CoverLetter,
                ResumePath = ja.ResumePath,
                ApplicationDate = ja.ApplicationDate.Date,
                ApplicationStatus = ja.ApplicationStatus.ToString()
            }).ToList();

            return jobApplicationDtos;
        }

        public async Task DeleteJobApplication(int id)
        {
            var application = await _dbContext.JobApplications.FindAsync(id);
            if(application == null)
            {
                throw new ArgumentException("Job Application not Found");
            }
            
            _dbContext.JobApplications.Remove(application);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateJobApplication(ApplyJobDto applyJobDto)
        {
            var jobApplication = await _dbContext.JobApplications
                .FirstOrDefaultAsync(ja => ja.SeekerId == applyJobDto.seekerId && ja.JobId == applyJobDto.jobId);

            if (jobApplication == null)
            {
                throw new ArgumentException("Job Application not found.");
            }

            var resume = await _dbContext.Resumes
                .FirstOrDefaultAsync(r => r.ResumeId == applyJobDto.resumeId && r.SeekerId == applyJobDto.seekerId);

            if (resume == null)
            {
                throw new ArgumentException("Invalid Resume ID.");
            }

            jobApplication.CoverLetter = applyJobDto.coverLetter;
            jobApplication.ApplicationDate = DateTime.UtcNow.Date; 
            jobApplication.ResumePath = resume.ResumePath;

            _dbContext.JobApplications.Update(jobApplication);
            await _dbContext.SaveChangesAsync();
        }

    }
}
