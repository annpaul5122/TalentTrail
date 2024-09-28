using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Enum;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly TalentTrailDbContext _dbContext;
        private readonly IEmailService _emailService;

        public JobSeekerService(TalentTrailDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task<JobSeeker> CreateProfile(JobSeeker jobSeeker, List<string> resumePaths, List<EducationDto> educations, List<CertificationDto> certifications)
        {
            var existingUser = await _dbContext.Users.FindAsync(jobSeeker.UserId);
            if (existingUser == null)
            {
                throw new ArgumentException("Invalid User ID.");
            }

            jobSeeker.CreatedAt = DateTime.UtcNow;

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

            foreach (var education in educations)
            {
                var edu = new Education
                {
                    Degree = education.Degree,
                    Institution = education.Institution,
                    PassoutYear = education.PassOutYear,
                    SeekerId = jobSeeker.SeekerId
                };
                _dbContext.Educations.Add(edu);
            }

            foreach (var certification in certifications)
            {
                var cert = new Certification
                {
                    CertificationName = certification.CertificationName,
                    CertificateImagePath = certification.CertificatePicturePath,
                    DateIssued = certification.DateIssued,
                    SeekerId = jobSeeker.SeekerId
                };
                _dbContext.Certifications.Add(cert);
            }

            await _dbContext.SaveChangesAsync();

            var subject = "Profile Creation - Talent Trail";
            var body = $"Hello {existingUser.FirstName},\n\nYour profile as an job seeker has been created successfully.";

            try
            {
                await _emailService.SendEmailAsync(existingUser.Email, subject, body);
            }
            catch (Exception)
            {
                
            }

            return jobSeeker;
        }

        public async Task<JobSeekerProfileDto> ViewProfile(int seekerId)
        {
            var jobSeeker = await _dbContext.JobSeekers
                .Include(js => js.User)
                .Include(js => js.Resumes)
                .Include(js => js.Application)
                .Include(js => js.Educations) 
                .Include(js => js.Certifications)
                .Include(js => js.Recommendations)
                .FirstOrDefaultAsync(js => js.SeekerId == seekerId);

            if (jobSeeker == null)
            {
                throw new ArgumentException("Job Seeker not found.");
            }

            var profileDto = new JobSeekerProfileDto
            {
                FirstName = jobSeeker.User.FirstName,
                LastName = jobSeeker.User.LastName,
                Email = jobSeeker.User.Email,
                PhoneNumber = jobSeeker.PhoneNumber,
                ProfileSummary = jobSeeker.ProfileSummary,
                ResumePath = jobSeeker.Resumes.Select(r => r.ResumePath).ToList(),
                Educations = jobSeeker.Educations.Select(e => new EducationDto
                {
                    Degree = e.Degree,
                    Institution = e.Institution,
                    PassOutYear = e.PassoutYear
                }).ToList(),
                Certifications = jobSeeker.Certifications.Select(c => new CertificationDto
                {
                    CertificationName = c.CertificationName,
                    CertificatePicturePath = c.CertificateImagePath,
                    DateIssued = c.DateIssued
                }).ToList(),
                Experience = jobSeeker.Experience,
                Skills = jobSeeker.Skills,
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
                .Include(js => js.Educations) 
                .Include(js => js.Certifications)
                .Include(js => js.Recommendations)
                .FirstOrDefaultAsync(js => js.SeekerId == seekerId);

            if (jobSeeker == null)
            {
                throw new ArgumentException("Job Seeker not found.");
            }

            _dbContext.Resumes.RemoveRange(jobSeeker.Resumes);
            _dbContext.JobApplications.RemoveRange(jobSeeker.Application);
            _dbContext.Recommendations.RemoveRange(jobSeeker.Recommendations);
            _dbContext.Educations.RemoveRange(jobSeeker.Educations);
            _dbContext.Certifications.RemoveRange(jobSeeker.Certifications);

            _dbContext.JobSeekers.Remove(jobSeeker);

            if (jobSeeker.User != null)
            {
                _dbContext.Users.Remove(jobSeeker.User);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<JobPostDto>> SearchJobPosts(string? jobTitle)
        {
            var query = _dbContext.JobPosts.AsQueryable();


            if (!string.IsNullOrEmpty(jobTitle))
            {
                query = query.Where(j => j.JobTitle.Contains(jobTitle));
            }


            var jobPosts = await query.Include(j => j.Employer)
                              .Include(j => j.Employer.Users) 
                              .ToListAsync();

            return jobPosts.Select(j => new JobPostDto
            {
                JobId=j.JobId,
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

        public async Task<List<JobPostDto>> JobPostFilter(string? jobTitle,string? industry,string? requirements,string? location,EmploymentType? employmentType)
        {
            var query = _dbContext.JobPosts.AsQueryable();

            if (!string.IsNullOrEmpty(jobTitle))
            {
                query = query.Where(j => j.JobTitle.Contains(jobTitle));
            }


            if (!string.IsNullOrEmpty(industry))
            {
                query = query.Where(j => j.Industry.Contains(industry));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(j => j.JobLocation.Equals(location));
            }


            if (employmentType.HasValue)
            {
                query = query.Where(j => j.EmploymentType == employmentType.Value);
            }


            if (!string.IsNullOrEmpty(requirements))
            {
                query = query.Where(j => j.JobRequirements.Contains(requirements));
            }

            var jobPosts = await query.Include(j => j.Employer)
                              .Include(j => j.Employer.Users)
                              .ToListAsync();

            return jobPosts.Select(j => new JobPostDto
            {
                JobId = j.JobId,
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
