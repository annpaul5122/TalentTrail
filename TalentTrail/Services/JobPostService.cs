using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Enum;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class JobPostService : IJobPostService
    {
        private readonly TalentTrailDbContext _context;
        private readonly IEmailService _emailService;
        public JobPostService(TalentTrailDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;

        }

        public async Task<JobPost> CreateJobPost(JobPost jobPost)
        {
            jobPost.CreatedAt = DateTime.Now;
            _context.JobPosts.Add(jobPost);
            await _context.SaveChangesAsync();
            return jobPost;
        }

        public async Task<JobPost> UpdateJobPost(int jobId, JobPost updatedJobPost)
        {
            var jobPost = await _context.JobPosts.FindAsync(jobId);

            if (jobPost == null)
            {
                throw new ArgumentException("Job post not found.");
            }

            jobPost.CompanyId = updatedJobPost.CompanyId;
            jobPost.JobTitle = updatedJobPost.JobTitle;
            jobPost.JobDescription = updatedJobPost.JobDescription;
            jobPost.JobRequirements = updatedJobPost.JobRequirements;
            jobPost.JobLocation = updatedJobPost.JobLocation;
            jobPost.SalaryRange = updatedJobPost.SalaryRange;
            jobPost.EmploymentType = updatedJobPost.EmploymentType;
            jobPost.Industry = updatedJobPost.Industry;
            jobPost.ApplicationDeadline = updatedJobPost.ApplicationDeadline.Date;
            jobPost.UpdatedAt = DateTime.Now;

            _context.JobPosts.Update(jobPost);
            await _context.SaveChangesAsync();

            return jobPost;
        }

        public async Task<JobPostDto> GetJobPostById(int jobId)
        {
            var jobPost = await _context.JobPosts
                .Include(j => j.Employer)
                .Include(j => j.Employer.Users)
                .Include(j=>j.CompanyDetails)
                .FirstOrDefaultAsync(j => j.JobId == jobId);

            if (jobPost == null)
            {
                throw new ArgumentException("Job post not found.");
            }

            var post = new JobPostDto()
            {
                JobId = jobId,
                EmployerName = jobPost.Employer.Users.FirstName + " " + jobPost.Employer.Users.LastName,
                CompanyName = jobPost.CompanyDetails.CompanyName,
                CompanyDescription = jobPost.CompanyDetails.CompanyDescription,
                CompanyLogo = jobPost.CompanyDetails.CompanyLogo,
                CompanyWebUrl = jobPost.CompanyDetails.CompanyWebUrl,
                JobTitle = jobPost.JobTitle,
                JobDescription = jobPost.JobDescription,
                JobRequirements = jobPost.JobRequirements,
                JobLocation = jobPost.JobLocation,
                SalaryRange = jobPost.SalaryRange,
                EmploymentType = jobPost.EmploymentType.ToString(),
                Industry = jobPost.Industry,
                CreatedAt = jobPost.CreatedAt,
                ApplicationDeadline = jobPost.ApplicationDeadline.Date,
                UpdatedAt = jobPost.UpdatedAt
            };

            return post;
        }

        public async Task<List<JobPostDto>> GetAllJobPosts()
        {
            var post= await _context.JobPosts.Include(j => j.Employer)
                .Include(j=>j.Employer.Users).Include(j => j.CompanyDetails).ToListAsync();

            var jobPostDtos = post.Select(j => new JobPostDto
            {
                JobId = j.JobId,
                EmployerName = j.Employer.Users.FirstName + " " + j.Employer.Users.LastName,
                CompanyName = j.CompanyDetails.CompanyName,
                CompanyDescription = j.CompanyDetails.CompanyDescription,
                CompanyLogo = j.CompanyDetails.CompanyLogo,
                CompanyWebUrl = j.CompanyDetails.CompanyWebUrl,
                JobTitle = j.JobTitle,
                JobDescription = j.JobDescription,
                JobRequirements = j.JobRequirements,
                JobLocation = j.JobLocation,
                SalaryRange = j.SalaryRange,
                EmploymentType = j.EmploymentType.ToString(),
                Industry = j.Industry,
                CreatedAt = j.CreatedAt,
                ApplicationDeadline = j.ApplicationDeadline.Date,
                UpdatedAt = j.UpdatedAt
            }).ToList();

            return jobPostDtos;
        }

        public async Task<List<JobPostDto>> GetJobPostsByEmployerId(int employerId)
        {
            var jobPosts = await _context.JobPosts
                     .Where(j => j.EmployerId == employerId)
                     .Include(j => j.Employer)
                     .Include(j => j.Employer.Users)
                     .Include(j => j.CompanyDetails)
                     .ToListAsync();

            var jobPostDtos = jobPosts.Select(j => new JobPostDto
            {
                JobId = j.JobId,
                EmployerName = j.Employer.Users.FirstName + " " + j.Employer.Users.LastName,
                CompanyName = j.CompanyDetails.CompanyName,
                CompanyDescription = j.CompanyDetails.CompanyDescription,
                CompanyLogo = j.CompanyDetails.CompanyLogo,
                CompanyWebUrl = j.CompanyDetails.CompanyWebUrl,
                JobTitle = j.JobTitle,
                JobDescription = j.JobDescription,
                JobRequirements = j.JobRequirements,
                JobLocation = j.JobLocation,
                SalaryRange = j.SalaryRange,
                EmploymentType = j.EmploymentType.ToString(),
                Industry=j.Industry,
                CreatedAt = j.CreatedAt,
                ApplicationDeadline = j.ApplicationDeadline.Date,
                UpdatedAt = j.UpdatedAt
            }).ToList();

            return jobPostDtos;
        }
        public async Task DeleteJobPost(int jobId)
        {
            var jobPost = await _context.JobPosts.FindAsync(jobId);
            if (jobPost == null)
            {
                throw new ArgumentException("Job post not found.");
            }

            _context.JobPosts.Remove(jobPost);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateApplicationStatus(int applicationId, ApplicationStatus newStatus)
        {
            var jobApplication = await _context.JobApplications
                .Include(x => x.jobSeeker.User)
                .Include(x=> x.jobPost)
                .FirstOrDefaultAsync(ja => ja.ApplicationId == applicationId);

            if (jobApplication == null)
            {
                throw new ArgumentException("Job Application not found.");
            }

            jobApplication.ApplicationStatus = newStatus;

            _context.JobApplications.Update(jobApplication);
            await _context.SaveChangesAsync();

            var subject = "Application Status - Talent Trail";
            var body = $"Hello {jobApplication.jobSeeker.User.FirstName},\n\nYour application for the job post {jobApplication.jobPost.JobTitle} has been viewed by the employer and the status has been changed to {newStatus.ToString()}.";

            try
            {
                await _emailService.SendEmailAsync(jobApplication.jobSeeker.User.Email, subject, body);
            }
            catch (Exception)
            {

            }
        }
    }
}
