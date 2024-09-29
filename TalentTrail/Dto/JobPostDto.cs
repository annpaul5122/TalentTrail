using System.ComponentModel.DataAnnotations;
using TalentTrail.Enum;

namespace TalentTrail.Dto
{
    public class JobPostDto
    {
        public int JobId { get; set; }
        public string EmployerName { get; set; }
        public string CompanyName { get; set; }
        public string? CompanyWebUrl { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyLogo { get; set; }
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set; }
        public string? JobRequirements { get; set; }
        public string? JobLocation { get; set; }
        public string? SalaryRange { get; set; }
        public string EmploymentType { get; set; }
        public string? Industry { get; set; }
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime ApplicationDeadline { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
