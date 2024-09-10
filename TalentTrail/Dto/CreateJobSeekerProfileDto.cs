using TalentTrail.Models;

namespace TalentTrail.Dto
{
    public class CreateJobSeekerProfileDto
    {
        public JobSeeker JobSeeker { get; set; }
        public List<string> ResumePaths { get; set; }
    }
}
