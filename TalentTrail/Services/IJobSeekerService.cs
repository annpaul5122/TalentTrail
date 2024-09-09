using TalentTrail.Dto;
using TalentTrail.Models;

public interface IJobSeekerService
{
    Task<JobSeeker> CreateProfile(JobSeeker jobSeeker);
    Task<JobSeekerProfileDto> ViewProfile(int seekerId);
    Task DeleteProfile(int seekerId);
}
