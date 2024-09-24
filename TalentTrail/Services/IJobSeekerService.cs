using TalentTrail.Dto;
using TalentTrail.Enum;
using TalentTrail.Models;

public interface IJobSeekerService
{
    public Task<JobSeeker> CreateProfile(JobSeeker jobSeeker, List<string> resumePaths, List<EducationDto> educations, List<CertificationDto> certifications);
    public Task<JobSeekerProfileDto> ViewProfile(int seekerId);
    public Task DeleteProfile(int seekerId);
    public Task<List<JobPostDto>> SearchJobPosts(string? industry, string? jobTitle, string? jobLocation,EmploymentType? employmentType);
}
