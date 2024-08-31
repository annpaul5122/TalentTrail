using TalentTrail.Enum;

namespace TalentTrail.Models
{
    public class JobPost
    {
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobRequirements { get; set; }
        public string JobLocation { get; set; }
        public string SalaryRange { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ApplicationDeadline { get; set; }  
        public DateTime? UpdatedAt { get; set; }

    }
}
