using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly TalentTrailDbContext _dbContext;
        private readonly IMapper _mapper;

        public JobApplicationService(TalentTrailDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<JobApplicationDto> CreateJobApplicationAsync(JobApplicationDto jobApplicationDto)
        {
            var jobApplication = _mapper.Map<JobApplication>(jobApplicationDto);
            _dbContext.JobApplications.Add(jobApplication);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<JobApplicationDto>(jobApplication);
        }

        public async Task<JobApplicationDto> GetJobApplicationByIdAsync(int id)
        {
            var jobApplication = await _dbContext.JobApplications
                .Include(j => j.jobPost)
                .Include(j => j.jobSeeker)
                .FirstOrDefaultAsync(j => j.ApplicationId == id);

            if (jobApplication == null)
            {
                return null;
            }

            return _mapper.Map<JobApplicationDto>(jobApplication);
        }

        public async Task<IEnumerable<JobApplicationDto>> GetAllJobApplicationsAsync()
        {
            var jobApplications = await _dbContext.JobApplications
                .Include(j => j.jobPost)
                .Include(j => j.jobSeeker)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobApplicationDto>>(jobApplications);
        }

        public async Task UpdateJobApplicationAsync(JobApplicationDto jobApplicationDto)
        {
            var jobApplication = _mapper.Map<JobApplication>(jobApplicationDto);
            _dbContext.JobApplications.Update(jobApplication);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteJobApplicationAsync(int id)
        {
            var jobApplication = await _dbContext.JobApplications.FindAsync(id);
            if (jobApplication != null)
            {
                _dbContext.JobApplications.Remove(jobApplication);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
