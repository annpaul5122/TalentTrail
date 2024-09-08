﻿using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class JobPostService : IJobPostService
    {
        private readonly TalentTrailDbContext _context;
        public JobPostService(TalentTrailDbContext context)
        {
            _context = context;
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
                throw new Exception("Job post not found.");
            }

            jobPost.JobTitle = updatedJobPost.JobTitle;
            jobPost.JobDescription = updatedJobPost.JobDescription;
            jobPost.JobRequirements = updatedJobPost.JobRequirements;
            jobPost.JobLocation = updatedJobPost.JobLocation;
            jobPost.SalaryRange = updatedJobPost.SalaryRange;
            jobPost.EmploymentType = updatedJobPost.EmploymentType;
            jobPost.ApplicationDeadline = updatedJobPost.ApplicationDeadline;
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
                .FirstOrDefaultAsync(j => j.JobId == jobId);

            if (jobPost == null)
            {
                throw new Exception("Job post not found.");
            }

            var post = new JobPostDto()
            {
                EmployerName = jobPost.Employer.Users.FirstName + " " + jobPost.Employer.Users.LastName,
                JobTitle = jobPost.JobTitle,
                JobDescription = jobPost.JobDescription,
                JobRequirements = jobPost.JobRequirements,
                JobLocation = jobPost.JobLocation,
                SalaryRange = jobPost.SalaryRange,
                EmploymentType = jobPost.EmploymentType.ToString(),
                CreatedAt = jobPost.CreatedAt,
                ApplicationDeadline = jobPost.ApplicationDeadline,
                UpdatedAt = jobPost.UpdatedAt
            };

            return post;
        }

        public async Task<List<JobPostDto>> GetAllJobPosts()
        {
            var post= await _context.JobPosts.Include(j => j.Employer)
                .Include(j=>j.Employer.Users).ToListAsync();

            var jobPostDtos = post.Select(j => new JobPostDto
            {
                EmployerName = j.Employer.Users.FirstName + " " + j.Employer.Users.LastName,
                JobTitle = j.JobTitle,
                JobDescription = j.JobDescription,
                JobRequirements = j.JobRequirements,
                JobLocation = j.JobLocation,
                SalaryRange = j.SalaryRange,
                EmploymentType = j.EmploymentType.ToString(),
                CreatedAt = j.CreatedAt,
                ApplicationDeadline = j.ApplicationDeadline,
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
                     .ToListAsync();

            var jobPostDtos = jobPosts.Select(j => new JobPostDto
            {
                EmployerName = j.Employer.Users.FirstName + " " + j.Employer.Users.LastName,
                JobTitle = j.JobTitle,
                JobDescription = j.JobDescription,
                JobRequirements = j.JobRequirements,
                JobLocation = j.JobLocation,
                SalaryRange = j.SalaryRange,
                EmploymentType = j.EmploymentType.ToString(),
                CreatedAt = j.CreatedAt,
                ApplicationDeadline = j.ApplicationDeadline,
                UpdatedAt = j.UpdatedAt
            }).ToList();

            return jobPostDtos;
        }
        public async Task DeleteJobPost(int jobId)
        {
            var jobPost = await _context.JobPosts.FindAsync(jobId);
            if (jobPost == null)
            {
                throw new Exception("Job post not found.");
            }

            _context.JobPosts.Remove(jobPost);
            await _context.SaveChangesAsync();
        }
    }
}
