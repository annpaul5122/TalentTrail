using System.Collections.Generic;
using System.Threading.Tasks;
using TalentTrail.Dto;

namespace TalentTrail.Services
{
    public interface IResumeService
    {
        Task<ResumeDto> CreateResumeAsync(ResumeDto resumeDto);
        Task<ResumeDto> GetResumeByIdAsync(int id);
        Task<IEnumerable<ResumeDto>> GetAllResumesAsync();
        Task UpdateResumeAsync(ResumeDto resumeDto);
        Task DeleteResumeAsync(int id);
    }
}
