using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Dto;
using TalentTrail.Models;

namespace TalentTrail.Services
{

    public class ResumeService : IResumeService
    {
        private readonly TalentTrailDbContext _dbContext;
        private readonly IMapper _mapper;

        public ResumeService(TalentTrailDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResumeDto> CreateResumeAsync(ResumeDto resumeDto)
        {
            var resume = _mapper.Map<Resume>(resumeDto);
            _dbContext.Resumes.Add(resume);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ResumeDto>(resume);
        }

        public async Task<ResumeDto> GetResumeByIdAsync(int id)
        {
            var resume = await _dbContext.Resumes
                .Include(r => r.JobSeeker)
                .FirstOrDefaultAsync(r => r.ResumeId == id);

            if (resume == null)
            {
                return null;
            }

            return _mapper.Map<ResumeDto>(resume);
        }

        public async Task<IEnumerable<ResumeDto>> GetAllResumesAsync()
        {
            var resumes = await _dbContext.Resumes
                .Include(r => r.JobSeeker)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ResumeDto>>(resumes);
        }

        public async Task UpdateResumeAsync(ResumeDto resumeDto)
        {
            var resume = _mapper.Map<Resume>(resumeDto);
            _dbContext.Resumes.Update(resume);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteResumeAsync(int id)
        {
            var resume = await _dbContext.Resumes.FindAsync(id);
            if (resume != null)
            {
                _dbContext.Resumes.Remove(resume);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
