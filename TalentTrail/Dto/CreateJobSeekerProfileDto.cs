using TalentTrail.Models;

namespace TalentTrail.Dto
{
    public class CreateJobSeekerProfileDto
    {
        public JobSeeker JobSeeker { get; set; }
        public List<string> ResumePaths { get; set; }
        public List<EducationDto> Educations { get; set; }
        public List<CertificationDto> Certifications { get; set; }
    }
}
