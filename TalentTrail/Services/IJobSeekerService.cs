using TalentTrail.Dto;
using TalentTrail.Enum;
using TalentTrail.Models;

public interface IJobSeekerService
{
    public Task<JobSeeker> CreateProfile(JobSeeker jobSeeker, List<string> resumePaths, List<EducationDto> educations, List<CertificationDto> certifications);
    public Task<JobSeekerProfileDto> ViewProfile(int seekerId);
    public Task DeleteProfile(int seekerId);
    public Task<List<JobPostDto>> SearchJobPosts(string? jobTitle);
    public Task<List<JobPostDto>> JobPostFilter(string? jobTitle,string? industry, string? requirements, string? location, EmploymentType? employmentType);

    public Task<JobSeeker> GetSeekerProfileByUserId(int userId);
    public Task<List<int>> GetAppliedJobsAsync(int seekerId);
}
